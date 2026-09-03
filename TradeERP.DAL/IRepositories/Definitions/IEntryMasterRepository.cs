using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IEntryMasterRepository : IGenericRepository<EntryMaster>
    {
        Task<PaginatedResult<EntryMaster>> GetPagedAsync(int pageNo, string? searchString);
        Task<ResultMessage> PostOpeningBalanceAsync(int ledgerAccountId, decimal amount, DebitCreditDirection direction, DateTime date);

        Task<EntryMaster?> GetByIdWithDetailsAsync(int id);
        Task<string> GetNewCodeAsync();
        Task<ResultMessage> AddWithLinesAsync(EntryMaster entry, List<EntryDetails> lines);
        Task<ResultMessage> UpdateWithLinesAsync(EntryMaster entry, List<EntryDetails> lines);
    }
}
