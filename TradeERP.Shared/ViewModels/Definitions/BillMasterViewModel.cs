using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class BillMasterViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public BillType BillType { get; set; }
        public decimal Amount { get; set; }
        public bool IsPosted { get; set; }

        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }

        public List<LookupItem> Customers { get; set; } = new List<LookupItem>();
        public List<LookupItem> Suppliers { get; set; } = new List<LookupItem>();
        public List<LookupItem> Products { get; set; } = new List<LookupItem>();

        public List<BillMasterLineViewModel> Lines { get; set; } = new List<BillMasterLineViewModel>();
    }
}
