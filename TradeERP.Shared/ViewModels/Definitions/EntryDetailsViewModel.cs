using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class EntryDetailsViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }

        public int EntryMasterId { get; set; }
        public string EntryMasterCode { get; set; } = string.Empty;

        public int LedgerAccountId { get; set; }
        public string LedgerAccountArName { get; set; } = string.Empty;
        public string LedgerAccountEnName { get; set; } = string.Empty;

        public List<LookupItem> EntryMasters { get; set; } = new List<LookupItem>();
        public List<LookupItem> LedgerAccounts { get; set; } = new List<LookupItem>();
    }
}
