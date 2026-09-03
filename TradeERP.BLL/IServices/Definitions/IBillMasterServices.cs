using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IBillMasterServices
    {
        Task<PaginatedResult<BillMasterViewModel>> GetPagedBillMasters(int pageNo, string? searchString);
        Task<BillMasterViewModel?> GetBillMasterById(int id);
        Task<ResultMessage> AddBillMaster(BillMasterViewModel viewModel);
        Task<ResultMessage> UpdateBillMaster(BillMasterViewModel viewModel);
        Task<ResultMessage> DeleteBillMaster(int id);
        Task<ResultMessage> PostBillMaster(int id);
    }
}
