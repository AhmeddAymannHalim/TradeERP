using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories;
using TradeERP.DAL.Repositories;

namespace TradeERP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDefinitionRepository? _definitionRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDefinitionRepository DefinitionRepository
            => _definitionRepository ??= new DefinitionRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
