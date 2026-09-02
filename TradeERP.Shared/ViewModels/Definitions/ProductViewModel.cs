using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public List<LookupItem> Categories { get; set; } = new List<LookupItem>();
    }
}
