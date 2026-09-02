namespace TradeERP.Shared.ViewModels.Commons
{
    public class AccountSettingsViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public ChangePasswordViewModel ChangePassword { get; set; } = new();
    }
}
