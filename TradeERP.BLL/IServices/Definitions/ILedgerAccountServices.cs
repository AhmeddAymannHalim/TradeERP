using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface ILedgerAccountServices
    {
        Task<PaginatedResult<LedgerAccountViewModel>> GetPagedLedgerAccounts(int pageNo, string? searchString);
        Task<LedgerAccountViewModel?> GetLedgerAccountById(int id);
        Task<int> GetNewLedgerAccountCodeAsync();
        Task<ResultMessage> AddLedgerAccount(LedgerAccountViewModel viewModel);
        Task<ResultMessage> UpdateLedgerAccount(LedgerAccountViewModel viewModel);
        Task<ResultMessage> DeleteLedgerAccount(int id);
    }
}
