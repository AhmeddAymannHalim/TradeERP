using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IEntrySettingServices
    {
        Task<PaginatedResult<EntrySettingViewModel>> GetPagedEntrySettings(int pageNo, string? searchString);
        Task<EntrySettingViewModel?> GetEntrySettingById(int id);
        Task<ResultMessage> AddEntrySetting(EntrySettingViewModel viewModel);
        Task<ResultMessage> UpdateEntrySetting(EntrySettingViewModel viewModel);
        Task<ResultMessage> DeleteEntrySetting(int id);
    }
}
