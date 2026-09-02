namespace TradeERP.Shared.ViewModels.Commons
{
    public class TokenResponseViewModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
