using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IEntryMasterServices
    {
        Task<PaginatedResult<EntryMasterViewModel>> GetPagedEntryMasters(int pageNo, string? searchString);
        Task<EntryMasterViewModel?> GetEntryMasterById(int id);
        Task<ResultMessage> AddEntryMaster(EntryMasterViewModel viewModel);
        Task<ResultMessage> UpdateEntryMaster(EntryMasterViewModel viewModel);
        Task<ResultMessage> DeleteEntryMaster(int id);
    }
}
