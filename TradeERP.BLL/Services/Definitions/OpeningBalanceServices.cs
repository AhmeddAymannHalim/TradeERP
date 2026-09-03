using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class OpeningBalanceServices : IOpeningBalanceServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpeningBalanceServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultMessage> PostOpeningBalance(OpeningBalanceViewModel viewModel)
        {
            return await _unitOfWork.EntryMasters.PostOpeningBalanceAsync(
                viewModel.LedgerAccountId, viewModel.Amount, viewModel.Direction, viewModel.Date);
        }
    }
}
