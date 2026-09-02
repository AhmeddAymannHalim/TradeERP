using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class EmployeeRepository : CodeDefinitionRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        
        }
    }
}
