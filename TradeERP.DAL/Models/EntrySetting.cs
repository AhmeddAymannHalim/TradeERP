namespace TradeERP.DAL.Models
{
    public class EntrySetting : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public int NextNumber { get; set; }
    }
}
