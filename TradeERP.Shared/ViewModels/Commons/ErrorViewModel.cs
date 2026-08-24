namespace TradeERP.Shared.ViewModels.Commons
{
    public class ErrorViewModel
    {
        public string? ErrorMessage { get; set; }

        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    }
}
