using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class SupplierRepository : CodeDefinitionRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<ResultMessage> AddAsync(Supplier entity)
        {
            if (entity.LedgerAccountId == null)
            {
                var ledgerAccount = new LedgerAccount
                {
                    Code = $"AP-{entity.Code}",
                    ArName = entity.ArName,
                    EnName = entity.EnName,
                    AccountType = AccountType.Liability
                };
                await _context.Set<LedgerAccount>().AddAsync(ledgerAccount);
                await _context.SaveChangesAsync();

                entity.LedgerAccountId = ledgerAccount.Id;
            }

            return await base.AddAsync(entity);
        }

        public override async Task<ResultMessage> UpdateAsync(Supplier entity)
        {
            if (entity.LedgerAccountId == null)
            {
                var ledgerAccount = new LedgerAccount
                {
                    Code = $"AP-{entity.Code}",
                    ArName = entity.ArName,
                    EnName = entity.EnName,
                    AccountType = AccountType.Liability
                };
                await _context.Set<LedgerAccount>().AddAsync(ledgerAccount);
                await _context.SaveChangesAsync();

                entity.LedgerAccountId = ledgerAccount.Id;
            }

            return await base.UpdateAsync(entity);
        }
    }
}
