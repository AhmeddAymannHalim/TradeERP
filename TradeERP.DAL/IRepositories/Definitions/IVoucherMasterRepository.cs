using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IVoucherMasterRepository : IGenericRepository<VoucherMaster>
    {
        Task<PaginatedResult<VoucherMaster>> GetPagedAsync(int pageNo, string? searchString);
        Task<ResultMessage> AddAndPostAsync(VoucherMaster voucher);
        Task<int> GetNewCodeAsync();
    }
}
