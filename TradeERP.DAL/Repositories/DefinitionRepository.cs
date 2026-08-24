using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories;

namespace TradeERP.DAL.Repositories
{
    // ---------------------------------------------------------------------
    // DefinitionRepository — implementation of IDefinitionRepository.
    // Uses ApplicationDbContext directly (no generic repository, no reflection).
    // Reads use AsNoTracking(); writes just call context.Add/Update/Remove
    // (SaveChangesAsync is invoked once, from IUnitOfWork).
    //
    // See IDefinitionRepository for the exact naming convention every future
    // entity method group in this class must follow.
    // ---------------------------------------------------------------------
    public class DefinitionRepository : IDefinitionRepository
    {
        private readonly ApplicationDbContext _context;

        public DefinitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Entity method groups (Employee, Department, ...) will be implemented here
        // as each Definitions module is added.
    }
}
