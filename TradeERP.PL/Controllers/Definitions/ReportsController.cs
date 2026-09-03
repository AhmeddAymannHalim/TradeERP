using Microsoft.AspNetCore.Mvc;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Definitions.Reports;

namespace TradeERP.PL.Controllers.Definitions
{
    public class ReportsController : Controller
    {
        private readonly IReportsServices _services;
        private readonly ILookupService _lookupService;

        public ReportsController(IReportsServices services, ILookupService lookupService)
        {
            _services = services;
            _lookupService = lookupService;
        }

        public async Task<IActionResult> TrialBalance(DateTime? fromDate, DateTime? toDate)
        {
            var rows = await _services.GetTrialBalance(fromDate, toDate);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return View(rows);
        }

        public async Task<IActionResult> StatementOfAccount(int? ledgerAccountId, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.LedgerAccounts = await _lookupService.LedgerAccountLookupAsync();
            ViewBag.LedgerAccountId = ledgerAccountId;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            var rows = ledgerAccountId.HasValue
                ? await _services.GetStatementOfAccount(ledgerAccountId.Value, fromDate, toDate)
                : new List<StatementOfAccountRowViewModel>();

            return View(rows);
        }

        public async Task<IActionResult> StockValuation()
        {
            var rows = await _services.GetStockValuation();
            return View(rows);
        }
    }
}
