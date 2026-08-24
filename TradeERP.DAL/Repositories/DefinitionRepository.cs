using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories;

namespace TradeERP.DAL.Repositories
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private readonly ApplicationDbContext _context;

        public DefinitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
