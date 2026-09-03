using TradeERP.Shared.ViewModels.Definitions;
using TradeERP.Shared.ViewModels.Definitions.Reports;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IReportsServices
    {
        Task<List<TrialBalanceRowViewModel>> GetTrialBalance(DateTime? fromDate, DateTime? toDate);
        Task<List<StatementOfAccountRowViewModel>> GetStatementOfAccount(int ledgerAccountId, DateTime? fromDate, DateTime? toDate);
        Task<List<StockValuationRowViewModel>> GetStockValuation();
        Task<List<BillMasterViewModel>> GetBillSalesAsync();
        Task<List<BillMasterViewModel>> GetBillSalesReturnAsync();
        Task<List<BillMasterViewModel>> GetBillPurchasesAsync();
        Task<List<BillMasterViewModel>> GetBillReturnPurchasesAsync();
        Task<DashboardSummaryViewModel> GetDashboardSummaryAsync();
    }
}
