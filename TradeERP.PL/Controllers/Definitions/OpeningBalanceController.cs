using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.PL.Controllers.Definitions
{
    public class OpeningBalanceController : Controller
    {
        private readonly IOpeningBalanceServices _services;
        private readonly ILookupService _lookupService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OpeningBalanceController(
            IOpeningBalanceServices services,
            ILookupService lookupService,
            IStringLocalizer<SharedResource> localizer)
        {
            _services = services;
            _lookupService = lookupService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Create()
        {
            var model = new OpeningBalanceViewModel
            {
                Date = DateTime.Today,
                LedgerAccounts = await _lookupService.LedgerAccountLookupAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OpeningBalanceViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.PostOpeningBalance(model);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileSaving"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["OpeningBalancePostedSuccessfully"].Value,
                redirectUrl = Url.Action("Index", "EntryMaster")
            });
        }
    }
}
