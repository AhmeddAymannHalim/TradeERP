using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class BillMasterRepository : GenericRepository<BillMaster>, IBillMasterRepository
    {
        private const int PageSize = 10;

        public BillMasterRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PaginatedResult<BillMaster>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(b => b.Code.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(b => b.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<BillMaster>
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
