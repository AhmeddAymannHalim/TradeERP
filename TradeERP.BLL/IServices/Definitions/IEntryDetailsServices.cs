using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IEntryDetailsServices
    {
        Task<PaginatedResult<EntryDetailsViewModel>> GetPagedEntryDetails(int pageNo, string? searchString);
        Task<EntryDetailsViewModel?> GetEntryDetailsById(int id);
        Task<ResultMessage> AddEntryDetails(EntryDetailsViewModel viewModel);
        Task<ResultMessage> UpdateEntryDetails(EntryDetailsViewModel viewModel);
        Task<ResultMessage> DeleteEntryDetails(int id);
    }
}
