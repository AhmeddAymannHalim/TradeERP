namespace TradeERP.Shared.ViewModels.Definitions
{
    public class JournalEntryLineViewModel
    {
        public string LedgerAccountArName { get; set; } = string.Empty;
        public string LedgerAccountEnName { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }

    public class JournalEntryViewModel
    {
        public string Code { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<JournalEntryLineViewModel> Lines { get; set; } = new List<JournalEntryLineViewModel>();
    }
}
