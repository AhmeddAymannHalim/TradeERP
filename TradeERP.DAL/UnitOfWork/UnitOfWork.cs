using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.DAL.Repositories.Definitions;

namespace TradeERP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDefinitionRepository? _definitions;
        private ILookupRepository? _lookups;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDefinitionRepository Definitions
            => _definitions ??= new DefinitionRepository(_context);

        public ILookupRepository Lookups
            => _lookups ??= new LookupRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
