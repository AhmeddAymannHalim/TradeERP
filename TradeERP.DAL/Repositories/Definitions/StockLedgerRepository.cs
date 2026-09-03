using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class StockLedgerRepository : GenericRepository<StockLedger>, IStockLedgerRepository
    {
        private const int PageSize = 20;

        public StockLedgerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PaginatedResult<StockLedger>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<StockLedger>().AsNoTracking()
                .Include(s => s.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(s =>
                    s.Product.ArName.ToLower().Contains(searchLower) ||
                    s.Product.EnName.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderByDescending(s => s.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<StockLedger>
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
