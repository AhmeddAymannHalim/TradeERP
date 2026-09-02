using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IBillDetailsServices
    {
        Task<PaginatedResult<BillDetailsViewModel>> GetPagedBillDetails(int pageNo, string? searchString);
        Task<BillDetailsViewModel?> GetBillDetailsById(int id);
        Task<ResultMessage> AddBillDetails(BillDetailsViewModel viewModel);
        Task<ResultMessage> UpdateBillDetails(BillDetailsViewModel viewModel);
        Task<ResultMessage> DeleteBillDetails(int id);
    }
}
