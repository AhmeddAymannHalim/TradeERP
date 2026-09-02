using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Commons
{
    public class CodeDefinitionRepository<T> : GenericRepository<T>, ICodeDefinitionRepository<T>
        where T : BaseEntity, ICodeDefinition
    {
        private const int PageSize = 10;

        public CodeDefinitionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PaginatedResult<T>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<T>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(a =>
                    a.Code.ToLower().Contains(searchLower) ||
                    a.ArName.ToLower().Contains(searchLower) ||
                    a.EnName.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(a => a.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }

        public async Task<int> GetNewCodeAsync()
        {
            var codes = await _context.Set<T>().Select(e => e.Code).ToListAsync();

            return codes
                .Where(c => int.TryParse(c, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }
    }
}
