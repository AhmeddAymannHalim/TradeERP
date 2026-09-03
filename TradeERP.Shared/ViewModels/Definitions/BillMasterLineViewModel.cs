namespace TradeERP.Shared.ViewModels.Definitions
{
    public class BillMasterLineViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
