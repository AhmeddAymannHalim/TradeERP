namespace TradeERP.Shared.ViewModels.Commons
{
    /// <summary>
    /// Generic dropdown/lookup item, used for Select2 sources etc.
    /// </summary>
    public class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class LookupItemStringId
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
