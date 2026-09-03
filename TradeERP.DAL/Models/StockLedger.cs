using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class StockLedger : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public DateTime MovementDate { get; set; }
        public StockMovementType MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }

        public StockSourceType SourceType { get; set; }
        public int SourceId { get; set; }
    }
}
