using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class VoucherMaster : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public DateTime VoucherDate { get; set; }
        public VoucherType VoucherType { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int TreasuryLedgerAccountId { get; set; }
        public LedgerAccount TreasuryLedgerAccount { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool IsPosted { get; set; }
    }
}
