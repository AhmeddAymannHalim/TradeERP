using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IVoucherMasterServices
    {
        Task<PaginatedResult<VoucherMasterViewModel>> GetPagedVoucherMasters(int pageNo, string? searchString);
        Task<VoucherMasterViewModel?> GetVoucherMasterById(int id);
        Task<ResultMessage> AddVoucherMaster(VoucherMasterViewModel viewModel);
        Task<ResultMessage> DeleteVoucherMaster(int id);
        Task<string> GetNewVoucherMasterCodeAsync();
    }
}
