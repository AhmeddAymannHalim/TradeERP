using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class VoucherMasterViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }

        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int TreasuryLedgerAccountId { get; set; }

        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsPosted { get; set; }

        public List<LookupItem> Customers { get; set; } = new List<LookupItem>();
        public List<LookupItem> Suppliers { get; set; } = new List<LookupItem>();
        public List<LookupItem> LedgerAccounts { get; set; } = new List<LookupItem>();
    }
}
