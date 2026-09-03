namespace TradeERP.Shared.ViewModels.Definitions.Reports
{
    public class StockValuationRowViewModel
    {
        public string ProductArName { get; set; } = string.Empty;
        public string ProductEnName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal AverageCost { get; set; }
        public decimal TotalValue { get; set; }
    }
}
