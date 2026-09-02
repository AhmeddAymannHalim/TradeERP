namespace TradeERP.Shared.ViewModels.Definitions
{
    public class BillSettingViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public int NextNumber { get; set; }
    }
}
