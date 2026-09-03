namespace TradeERP.Shared.ViewModels.Definitions.Reports
{
    public class TrialBalanceRowViewModel
    {
        public string LedgerAccountArName { get; set; } = string.Empty;
        public string LedgerAccountEnName { get; set; } = string.Empty;
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
    }
}
