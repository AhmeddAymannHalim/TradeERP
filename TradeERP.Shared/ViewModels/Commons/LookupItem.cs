namespace TradeERP.Shared.ViewModels.Commons
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
    }

    public class LookupItemStringId
    {
        public string Id { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
    }
}
