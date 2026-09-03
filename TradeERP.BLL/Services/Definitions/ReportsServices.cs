using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Definitions.Reports;

namespace TradeERP.BLL.Services.Definitions
{
    public class ReportsServices : IReportsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TrialBalanceRowViewModel>> GetTrialBalance(DateTime? fromDate, DateTime? toDate)
        {
            return await _unitOfWork.Reports.GetTrialBalanceAsync(fromDate, toDate);
        }

        public async Task<List<StatementOfAccountRowViewModel>> GetStatementOfAccount(int ledgerAccountId, DateTime? fromDate, DateTime? toDate)
        {
            return await _unitOfWork.Reports.GetStatementOfAccountAsync(ledgerAccountId, fromDate, toDate);
        }

        public async Task<List<StockValuationRowViewModel>> GetStockValuation()
        {
            return await _unitOfWork.Reports.GetStockValuationAsync();
        }
    }
}
