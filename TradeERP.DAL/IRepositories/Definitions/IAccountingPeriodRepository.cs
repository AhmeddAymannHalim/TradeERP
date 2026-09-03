using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IAccountingPeriodRepository : IGenericRepository<AccountingPeriod>
    {
        Task<PaginatedResult<AccountingPeriod>> GetPagedAsync(int pageNo, string? searchString);
        Task<ResultMessage> CloseAsync(int id);
        Task<bool> IsDateInClosedPeriodAsync(DateTime date);
    }
}
