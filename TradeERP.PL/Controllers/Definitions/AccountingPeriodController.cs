using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.PL.Controllers.Definitions
{
    public class AccountingPeriodController : Controller
    {
        private readonly IAccountingPeriodServices _services;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AccountingPeriodController(
            IAccountingPeriodServices services,
            IStringLocalizer<SharedResource> localizer)
        {
            _services = services;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string searchString, int pageNo = 1)
        {
            try
            {
                var result = await _services.GetPagedAccountingPeriods(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public IActionResult Create()
        {
            return View(new AccountingPeriodViewModel { StartDate = DateTime.Today, EndDate = DateTime.Today });
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountingPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddAccountingPeriod(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message ?? _localizer["ErrorWhileSaving"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["SavedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Close(int id)
        {
            var result = await _services.CloseAccountingPeriod(id);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileSaving"].Value });

            return Json(new { success = true, message = _localizer["PeriodClosedSuccessfully"].Value });
        }
    }
}
