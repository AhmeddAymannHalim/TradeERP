using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class OpeningBalanceViewModel
    {
        public int LedgerAccountId { get; set; }
        public decimal Amount { get; set; }
        public DebitCreditDirection Direction { get; set; }
        public DateTime Date { get; set; }

        public List<LookupItem> LedgerAccounts { get; set; } = new List<LookupItem>();
    }
}
