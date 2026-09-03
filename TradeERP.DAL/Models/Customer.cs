namespace TradeERP.DAL.Models
{
    public class Customer : BaseEntity, ICodeDefinition
    {
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public int? LedgerAccountId { get; set; }
        public LedgerAccount? LedgerAccount { get; set; }
    }
}
