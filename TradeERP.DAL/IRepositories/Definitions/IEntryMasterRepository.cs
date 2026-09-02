using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IEntryMasterRepository : IGenericRepository<EntryMaster>
    {
        Task<PaginatedResult<EntryMaster>> GetPagedAsync(int pageNo, string? searchString);
    }
}
