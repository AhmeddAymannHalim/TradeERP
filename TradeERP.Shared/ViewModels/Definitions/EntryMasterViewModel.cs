using TradeERP.Shared.Enums;

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
    }
}
