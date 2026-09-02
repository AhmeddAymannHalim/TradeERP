using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IEntryDetailsRepository : IGenericRepository<EntryDetails>
    {
        Task<PaginatedResult<EntryDetails>> GetPagedAsync(int pageNo, string? searchString);
    }
}
