namespace TradeERP.Shared.ViewModels.Definitions.Reports
{
    public class DashboardSummaryViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalEmployees { get; set; }

        public decimal TodaySalesAmount { get; set; }
        public decimal MonthSalesAmount { get; set; }
        public decimal MonthPurchasesAmount { get; set; }
        public decimal TotalStockValue { get; set; }

        public List<BillMasterViewModel> RecentSales { get; set; } = new List<BillMasterViewModel>();
        public List<BillMasterViewModel> RecentPurchases { get; set; } = new List<BillMasterViewModel>();
        public List<StockValuationRowViewModel> TopStockValueProducts { get; set; } = new List<StockValuationRowViewModel>();
    }
}
