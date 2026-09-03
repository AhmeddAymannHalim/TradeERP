namespace TradeERP.Shared.ViewModels.Definitions
{
    public class EntryLineViewModel
    {
        public int Id { get; set; }
        public int LedgerAccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
