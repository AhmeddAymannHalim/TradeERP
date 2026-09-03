using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.PL.Controllers.Common;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IReportsServices _reportsServices;

    public HomeController(ILogger<HomeController> logger, IReportsServices reportsServices)
    {
        _logger = logger;
        _reportsServices = reportsServices;
    }

    public async Task<IActionResult> Index()
    {
        var summary = await _reportsServices.GetDashboardSummaryAsync();
        return View(summary);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        return LocalRedirect(returnUrl);
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
