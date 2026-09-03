using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IOpeningBalanceServices
    {
        Task<ResultMessage> PostOpeningBalance(OpeningBalanceViewModel viewModel);
    }
}
