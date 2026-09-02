namespace TradeERP.DAL.Models
{
    public class EntryDetails : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public int EntryMasterId { get; set; }
        public EntryMaster EntryMaster { get; set; } = null!;

        public int LedgerAccountId { get; set; }
        public LedgerAccount LedgerAccount { get; set; } = null!;

        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
