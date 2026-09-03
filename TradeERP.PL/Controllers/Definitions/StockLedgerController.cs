using Microsoft.AspNetCore.Mvc;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.PL.Controllers.Definitions
{
    public class StockLedgerController : Controller
    {
        private readonly IStockLedgerServices _services;

        public StockLedgerController(IStockLedgerServices services)
        {
            _services = services;
        }

        public async Task<IActionResult> Index(string searchString, int pageNo = 1)
        {
            try
            {
                var result = await _services.GetPagedStockLedger(pageNo, searchString);
                return View(result);
            }
            catch (Exception ex)
            {
                return ErrorHandler.ErrorView(ex.Message);
            }
        }
    }
}
