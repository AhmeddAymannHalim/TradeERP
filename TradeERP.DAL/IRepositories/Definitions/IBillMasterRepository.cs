using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IBillMasterRepository : IGenericRepository<BillMaster>
    {
        Task<PaginatedResult<BillMaster>> GetPagedAsync(int pageNo, string? searchString);
        Task<string> GetNewCodeAsync();
        Task<BillMaster?> GetByIdWithDetailsAsync(int id);
        Task<ResultMessage> PostBillAsync(int id);
        Task<ResultMessage> AddWithDetailsAsync(BillMaster bill, List<BillDetails> lines);
        Task<ResultMessage> UpdateWithDetailsAsync(BillMaster bill, List<BillDetails> lines);
    }
}
