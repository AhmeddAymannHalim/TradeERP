using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class Product : BaseEntity, ICodeDefinition
    {
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ProductUnit Unit { get; set; }
        public decimal Price { get; set; }
    }
}
