using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class AccountingPeriodRepository : GenericRepository<AccountingPeriod>, IAccountingPeriodRepository
    {
        private const int PageSize = 10;

        public AccountingPeriodRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PaginatedResult<AccountingPeriod>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<AccountingPeriod>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderByDescending(p => p.StartDate)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<AccountingPeriod>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }

        public async Task<ResultMessage> CloseAsync(int id)
        {
            var period = await _context.Set<AccountingPeriod>().FindAsync(id);
            if (period == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (period.IsClosed)
                return new ResultMessage { Success = false, Message = "PeriodAlreadyClosed" };

            period.IsClosed = true;
            await _context.SaveChangesAsync();
            return new ResultMessage { Success = true };
        }

        public async Task<bool> IsDateInClosedPeriodAsync(DateTime date)
        {
            return await _context.Set<AccountingPeriod>()
                .AsNoTracking()
                .AnyAsync(p => p.IsClosed && date >= p.StartDate && date <= p.EndDate);
        }
    }
}
