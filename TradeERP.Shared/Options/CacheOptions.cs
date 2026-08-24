namespace TradeERP.Shared.Options
{
    public class CacheOptions
    {
        public string Prefix { get; set; } = string.Empty;
        public int SlidingExpirationMinutes { get; set; } = 30;
        public int AbsoluteExpirationRelativeToNowMinutes { get; set; } = 120;
    }
}
