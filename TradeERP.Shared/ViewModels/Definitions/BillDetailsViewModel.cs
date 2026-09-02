using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class BillDetailsViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public int BillMasterId { get; set; }
        public string BillMasterCode { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public string ProductArName { get; set; } = string.Empty;
        public string ProductEnName { get; set; } = string.Empty;

        public List<LookupItem> BillMasters { get; set; } = new List<LookupItem>();
        public List<LookupItem> Products { get; set; } = new List<LookupItem>();
    }
}
