using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IBillSettingRepository : IGenericRepository<BillSetting>
    {
        Task<PaginatedResult<BillSetting>> GetPagedAsync(int pageNo, string? searchString);
    }
}
