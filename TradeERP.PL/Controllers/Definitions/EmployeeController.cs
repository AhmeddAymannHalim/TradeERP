using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.PL.Controllers.Definitions
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _services;
        private readonly ILookupService _lookupService;
        private readonly IToastrService _toastr;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public EmployeeController(
            IEmployeeServices services,
            ILookupService lookupService,
            IStringLocalizer<SharedResource> localizer,
            IToastrService toastr)
        {
            _services = services;
            _lookupService = lookupService;
            _localizer = localizer;
            _toastr = toastr;
        }

        public async Task<IActionResult> Index(string searchString, int pageNo = 1)
        {
            try
            {
                var result = await _services.GetPagedEmployees(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var newCode = await _services.GetNewEmployeeCodeAsync();
            var viewModel = new EmployeeViewModel
            {
                Code = newCode.ToString()
            };

            await PopulateData(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelStateErrors(), message = _localizer["ValidationError"].Value });

            var result = await _services.AddEmployee(model);
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
            var model = await _services.GetEmployeeById(id);
            if (model == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Index");
            }

            await PopulateData(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelStateErrors(), message = _localizer["ValidationError"].Value });

            var result = await _services.UpdateEmployee(model);
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
                var result = await _services.DeleteEmployee(id);
                if (result.Success)
                    return Json(new { success = true });

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        private async Task PopulateData(EmployeeViewModel model)
        {
            model.Specializations = await _lookupService.SpecializationLookupAsync();
            model.Countries = await _lookupService.CountryLookupAsync();

            if (model.CountryId > 0)
                model.Govs = await _lookupService.GovLookupByCountryIdAsync(model.CountryId.Value);

            if (model.GovId > 0)
                model.Towns = await _lookupService.TownLookupByGovIdAsync(model.GovId.Value);

            if (model.TownId > 0)
                model.Villages = await _lookupService.VillageLookupByTownIdAsync(model.TownId.Value);
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
