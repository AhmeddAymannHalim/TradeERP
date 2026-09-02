using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class SpecializationRepository : CodeDefinitionRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(ApplicationDbContext context) : base(context) { }
    }
}
