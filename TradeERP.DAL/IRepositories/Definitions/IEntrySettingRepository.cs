using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IEntrySettingRepository : IGenericRepository<EntrySetting>
    {
        Task<PaginatedResult<EntrySetting>> GetPagedAsync(int pageNo, string? searchString);
    }
}
