using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class EntryDetailsRepository : GenericRepository<EntryDetails>, IEntryDetailsRepository
    {
        private const int PageSize = 10;

        public EntryDetailsRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<ResultMessage> AddAsync(EntryDetails entity)
        {
            if (await IsParentEntrySystemGeneratedAsync(entity.EntryMasterId))
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            return await base.AddAsync(entity);
        }

        public override async Task<ResultMessage> UpdateAsync(EntryDetails entity)
        {
            if (await IsParentEntrySystemGeneratedAsync(entity.EntryMasterId))
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            return await base.UpdateAsync(entity);
        }

        public override async Task<ResultMessage> DeleteAsync(int id)
        {
            var existing = await _context.Set<EntryDetails>().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (await IsParentEntrySystemGeneratedAsync(existing.EntryMasterId))
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            return await base.DeleteAsync(id);
        }

        private async Task<bool> IsParentEntrySystemGeneratedAsync(int entryMasterId)
        {
            var entryType = await _context.Set<EntryMaster>()
                .AsNoTracking()
                .Where(m => m.Id == entryMasterId)
                .Select(m => m.EntryType)
                .FirstOrDefaultAsync();

            return entryType != TradeERP.Shared.Enums.EntryType.Manual;
        }

        public async Task<PaginatedResult<EntryDetails>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<EntryDetails>().AsNoTracking()
                .Include(d => d.EntryMaster)
                .Include(d => d.LedgerAccount)
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

            return new PaginatedResult<EntryDetails>
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
