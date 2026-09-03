using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class BillMaster : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public BillType BillType { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public decimal Amount { get; set; }
        public bool IsPosted { get; set; }

        public ICollection<BillDetails> BillDetails { get; set; } = new List<BillDetails>();
    }
}
