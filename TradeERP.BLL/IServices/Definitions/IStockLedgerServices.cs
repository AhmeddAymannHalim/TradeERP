using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IStockLedgerServices
    {
        Task<PaginatedResult<StockLedgerViewModel>> GetPagedStockLedger(int pageNo, string? searchString);
    }
}
