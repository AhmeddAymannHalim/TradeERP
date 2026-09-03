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
    public class VoucherMasterController : Controller
    {
        private readonly IVoucherMasterServices _services;
        private readonly ILookupService _lookupService;
        private readonly IToastrService _toastr;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VoucherMasterController(
            IVoucherMasterServices services,
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
                var result = await _services.GetPagedVoucherMasters(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var newCode = await _services.GetNewVoucherMasterCodeAsync();
            var model = new VoucherMasterViewModel
            {
                Code = newCode,
                VoucherDate = DateTime.Today
            };

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherMasterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddVoucherMaster(model);
            if (!result.Success)
                return Json(new { success = false, message = _localizer[result.Message ?? "ErrorWhileSaving"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["SavedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _services.DeleteVoucherMaster(id);
                if (result.Success)
                    return Json(new { success = true });

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        private async Task PopulateData(VoucherMasterViewModel model)
        {
            model.Customers = await _lookupService.CustomerLookupAsync();
            model.Suppliers = await _lookupService.SupplierLookupAsync();
            model.LedgerAccounts = await _lookupService.LedgerAccountLookupAsync();
        }
    }
}
