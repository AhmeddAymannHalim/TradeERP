using TradeERP.Shared.Enums;
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
        public ProductUnit Unit { get; set; }
        public decimal Price { get; set; }

        public List<LookupItem> Categories { get; set; } = new List<LookupItem>();
    }
}
