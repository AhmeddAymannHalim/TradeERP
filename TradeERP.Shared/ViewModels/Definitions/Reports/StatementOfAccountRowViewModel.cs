namespace TradeERP.Shared.ViewModels.Definitions.Reports
{
    public class StatementOfAccountRowViewModel
    {
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal RunningBalance { get; set; }
    }
}
