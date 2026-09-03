using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Definitions;
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

        public async Task<List<BillMasterViewModel>> GetBillSalesAsync()
        {
            return await _unitOfWork.Reports.GetBillSalesAsync();
        }

        public async Task<List<BillMasterViewModel>> GetBillSalesReturnAsync()
        {
            return await _unitOfWork.Reports.GetBillSalesReturnAsync();
        }
        public async Task<List<BillMasterViewModel>> GetBillPurchasesAsync()
        {
            return await _unitOfWork.Reports.GetBillPurchasesAsync();
        }
        
        public async Task<List<BillMasterViewModel>> GetBillReturnPurchasesAsync()
        {
            return await _unitOfWork.Reports.GetBillReturnPurchasesAsync();
        }

        public async Task<DashboardSummaryViewModel> GetDashboardSummaryAsync()
        {
            return await _unitOfWork.Reports.GetDashboardSummaryAsync();
        }
    }
}
