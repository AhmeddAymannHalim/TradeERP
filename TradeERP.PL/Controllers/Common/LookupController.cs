using Microsoft.AspNetCore.Mvc;
using TradeERP.BLL.IServices.ICommons;

namespace TradeERP.PL.Controllers.Common
{
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        public async Task<IActionResult> GetGovsByCountryId(int countryId)
        {
            var govs = await _lookupService.GovLookupByCountryIdAsync(countryId);
            return Json(govs);
        }

        public async Task<IActionResult> GetTownsByGovId(int govId)
        {
            var towns = await _lookupService.TownLookupByGovIdAsync(govId);
            return Json(towns);
        }

        public async Task<IActionResult> GetVillagesByTownId(int townId)
        {
            var villages = await _lookupService.VillageLookupByTownIdAsync(townId);
            return Json(villages);
        }
    }
}
