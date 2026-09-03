using TradeERP.Shared.ViewModels.Definitions;
using TradeERP.Shared.ViewModels.Definitions.Reports;

namespace TradeERP.DAL.IRepositories.ICommons
{
    public interface IReportsRepository
    {
        Task<List<TrialBalanceRowViewModel>> GetTrialBalanceAsync(DateTime? fromDate, DateTime? toDate);
        Task<List<StatementOfAccountRowViewModel>> GetStatementOfAccountAsync(int ledgerAccountId, DateTime? fromDate, DateTime? toDate);
        Task<List<StockValuationRowViewModel>> GetStockValuationAsync();
        Task<List<BillMasterViewModel>> GetBillSalesAsync();
        Task<List<BillMasterViewModel>> GetBillSalesReturnAsync();
        Task<List<BillMasterViewModel>> GetBillPurchasesAsync();
        Task<List<BillMasterViewModel>> GetBillReturnPurchasesAsync();
        Task<DashboardSummaryViewModel> GetDashboardSummaryAsync();
    }
}
