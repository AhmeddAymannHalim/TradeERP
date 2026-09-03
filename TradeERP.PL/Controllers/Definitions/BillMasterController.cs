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
    public class BillMasterController : Controller
    {
        private readonly IBillMasterServices _services;
        private readonly ILookupService _lookupService;
        private readonly IToastrService _toastr;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BillMasterController(
            IBillMasterServices services,
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
                var result = await _services.GetPagedBillMasters(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var newCode = await _services.GetNewBillMasterCodeAsync();
            var model = new BillMasterViewModel
            {
                Code = newCode,
                BillDate = DateTime.Today
            };

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillMasterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddBillMaster(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message ?? _localizer["ErrorWhileSaving"].Value });

            var message = result.Data != null
                ? _localizer["SavedButNotPosted"].Value + " " + _localizer[result.Data].Value
                : _localizer["SavedAndPostedSuccessfully"].Value;

            return Json(new
            {
                success = true,
                message,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _services.GetBillMasterById(id);
            if (model == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Index");
            }

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BillMasterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.UpdateBillMaster(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message ?? _localizer["ErrorWhileUpdating"].Value });

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
                var result = await _services.DeleteBillMaster(id);
                if (result.Success)
                    return Json(new { success = true });

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        public async Task<IActionResult> Post(int id)
        {
            var result = await _services.PostBillMaster(id);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileSaving"].Value });

            return Json(new { success = true, message = _localizer["PostedSuccessfully"].Value });
        }

        public async Task<IActionResult> JournalEntry(int id)
        {
            var entry = await _services.GetJournalEntryForBill(id);
            if (entry == null)
                return Json(new { success = false, message = _localizer["RecordNotFound"].Value });

            return Json(new { success = true, entry });
        }

        private async Task PopulateData(BillMasterViewModel model)
        {
            model.Customers = await _lookupService.CustomerLookupAsync();
            model.Suppliers = await _lookupService.SupplierLookupAsync();
            model.Products = await _lookupService.ProductLookupAsync();
        }
    }
}
