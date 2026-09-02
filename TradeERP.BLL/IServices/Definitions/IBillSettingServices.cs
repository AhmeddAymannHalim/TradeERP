using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IBillSettingServices
    {
        Task<PaginatedResult<BillSettingViewModel>> GetPagedBillSettings(int pageNo, string? searchString);
        Task<BillSettingViewModel?> GetBillSettingById(int id);
        Task<ResultMessage> AddBillSetting(BillSettingViewModel viewModel);
        Task<ResultMessage> UpdateBillSetting(BillSettingViewModel viewModel);
        Task<ResultMessage> DeleteBillSetting(int id);
    }
}
