using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class LedgerAccount : BaseEntity, ICodeDefinition
    {
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public AccountType AccountType { get; set; }

        public ICollection<EntryDetails> EntryDetails { get; set; } = new List<EntryDetails>();
    }
}
