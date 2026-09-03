using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class LedgerAccountRepository : CodeDefinitionRepository<LedgerAccount>, ILedgerAccountRepository
    {
        public LedgerAccountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<LedgerAccount?> GetByCodeAsync(string code)
        {
            return await _context.Set<LedgerAccount>().FirstOrDefaultAsync(a => a.Code == code);
        }
    }
}
