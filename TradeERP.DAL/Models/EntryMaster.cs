namespace TradeERP.DAL.Models
{
    public class EntryMaster : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public ICollection<EntryDetails> EntryDetails { get; set; } = new List<EntryDetails>();
    }
}
