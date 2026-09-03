using TradeERP.Shared.Enums;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class StockLedgerViewModel
    {
        public int Id { get; set; }
        public string ProductArName { get; set; } = string.Empty;
        public string ProductEnName { get; set; } = string.Empty;

        public DateTime MovementDate { get; set; }
        public StockMovementType MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }

        public StockSourceType SourceType { get; set; }
        public int SourceId { get; set; }
    }
}
