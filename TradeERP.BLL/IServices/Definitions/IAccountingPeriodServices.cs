using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IAccountingPeriodServices
    {
        Task<PaginatedResult<AccountingPeriodViewModel>> GetPagedAccountingPeriods(int pageNo, string? searchString);
        Task<ResultMessage> AddAccountingPeriod(AccountingPeriodViewModel viewModel);
        Task<ResultMessage> CloseAccountingPeriod(int id);
    }
}
