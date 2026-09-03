using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface ILedgerAccountRepository : ICodeDefinitionRepository<LedgerAccount>
    {
        Task<LedgerAccount?> GetByCodeAsync(string code);
    }
}
