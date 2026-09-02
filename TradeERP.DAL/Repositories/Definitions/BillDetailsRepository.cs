using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class BillDetailsRepository : GenericRepository<BillDetails>, IBillDetailsRepository
    {
        private const int PageSize = 10;

        public BillDetailsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PaginatedResult<BillDetails>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<BillDetails>().AsNoTracking()
                .Include(d => d.BillMaster)
                .Include(d => d.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(d => d.Code.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<BillDetails>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }
    }
}
