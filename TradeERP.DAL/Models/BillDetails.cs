namespace TradeERP.DAL.Models
{
    public class BillDetails : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public int BillMasterId { get; set; }
        public BillMaster BillMaster { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
