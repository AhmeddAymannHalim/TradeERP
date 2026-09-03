using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class EntryMasterViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public EntryType EntryType { get; set; } = EntryType.Manual;
        public int? SourceBillMasterId { get; set; }

        public List<LookupItem> LedgerAccounts { get; set; } = new List<LookupItem>();
        public List<EntryLineViewModel> Lines { get; set; } = new List<EntryLineViewModel>();
    }
}
