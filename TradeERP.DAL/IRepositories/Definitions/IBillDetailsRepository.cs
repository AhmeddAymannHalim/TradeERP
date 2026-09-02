using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IBillDetailsRepository : IGenericRepository<BillDetails>
    {
        Task<PaginatedResult<BillDetails>> GetPagedAsync(int pageNo, string? searchString);
    }
}
