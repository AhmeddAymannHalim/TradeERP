namespace TradeERP.DAL.Models
{
    public class EntryMaster : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public int? SourceBillMasterId { get; set; }
        public BillMaster? SourceBillMaster { get; set; }

        public int? SourceVoucherMasterId { get; set; }
        public VoucherMaster? SourceVoucherMaster { get; set; }

        public ICollection<EntryDetails> EntryDetails { get; set; } = new List<EntryDetails>();
    }
}
