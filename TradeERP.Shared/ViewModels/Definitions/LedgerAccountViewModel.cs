namespace TradeERP.Shared.ViewModels.Definitions
{
    public class LedgerAccountViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
    }
}
