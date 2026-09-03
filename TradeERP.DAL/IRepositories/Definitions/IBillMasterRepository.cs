using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IBillMasterRepository : IGenericRepository<BillMaster>
    {
        Task<PaginatedResult<BillMaster>> GetPagedAsync(int pageNo, string? searchString);
        Task<BillMaster?> GetByIdWithDetailsAsync(int id);
        Task<ResultMessage> PostBillAsync(int id);
    }
}
