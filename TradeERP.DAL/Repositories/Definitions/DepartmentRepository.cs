using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class DepartmentRepository : CodeDefinitionRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }
    }
}
