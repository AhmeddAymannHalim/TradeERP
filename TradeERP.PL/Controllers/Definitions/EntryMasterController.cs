using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.PL.Controllers.Definitions
{
    public class EntryMasterController : Controller
    {
        private readonly IEntryMasterServices _services;
        private readonly ILookupService _lookupService;
        private readonly IToastrService _toastr;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public EntryMasterController(
            IEntryMasterServices services,
            ILookupService lookupService,
            IToastrService toastr,
            IStringLocalizer<SharedResource> localizer)
        {
            _services = services;
            _lookupService = lookupService;
            _toastr = toastr;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string searchString, int pageNo = 1)
        {
            try
            {
                var result = await _services.GetPagedEntryMasters(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var newCode = await _services.GetNewEntryMasterCodeAsync();
            var model = new EntryMasterViewModel
            {
                Code = newCode,
                EntryDate = DateTime.Today
            };

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EntryMasterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddEntryMaster(model);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileSaving"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["SavedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _services.GetEntryMasterById(id);
            if (model == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Index");
            }

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EntryMasterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.UpdateEntryMaster(model);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileUpdating"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["UpdatedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _services.DeleteEntryMaster(id);
                if (result.Success)
                    return Json(new { success = true });

                return Json(new { success = false, message = _localizer[result.Message ?? "DeleteFailed"].Value });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        private async Task PopulateData(EntryMasterViewModel model)
        {
            model.LedgerAccounts = await _lookupService.LedgerAccountLookupAsync();
        }
    }
}
