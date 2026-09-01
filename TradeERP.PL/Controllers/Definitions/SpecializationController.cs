using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.Shared;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.PL.Controllers.Definitions
{
    public class SpecializationController : Controller
    {
        private readonly ISpecializationServices _services;
        private readonly IToastrService _toastr;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SpecializationController(
            ISpecializationServices services,
            IToastrService toastr,
            IStringLocalizer<SharedResource> localizer)
        {
            _services = services;
            _toastr = toastr;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string searchString, int pageNo = 1)
        {
            try
            {
                var result = await _services.GetPagedSpecializations(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var newCode = await _services.GetNewSpecializationCodeAsync();
            var model = new SpecializationViewModel
            {
                Code = newCode.ToString()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecializationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelStateErrors(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddSpecialization(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message ?? _localizer["ErrorWhileSaving"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["SavedSuccessfully"].Value,
                redirectUrl = Url.Action("Index", new { searchString = model.ArName })
            });
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _services.GetSpecializationById(id);
            if (model == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpecializationViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelStateErrors(), message = _localizer["ValidationError"].Value });

            var result = await _services.UpdateSpecialization(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message ?? _localizer["ErrorWhileUpdating"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["UpdatedSuccessfully"].Value,
                redirectUrl = Url.Action("Index", new { searchString = model.ArName })
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _services.DeleteSpecialization(id);
                if (result.Success)
                    return Json(new { success = true });

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        private Dictionary<string, string[]> ModelStateErrors()
        {
            return ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
        }
    }
}
