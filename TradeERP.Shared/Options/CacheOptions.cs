namespace TradeERP.Shared.Options
{
    public class CacheOptions
    {
        public const string SectionName = "CacheOptions";

        public int SlidingExpirationMinutes { get; set; } = 30;
        public int AbsoluteExpirationRelativeToNowMinutes { get; set; } = 120;
    }
}
