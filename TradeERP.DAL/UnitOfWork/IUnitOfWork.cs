using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.IRepositories.Definitions;

namespace TradeERP.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        ISpecializationRepository Specializations { get; }
        IDepartmentRepository Departments { get; }
        ILookupRepository Lookups { get; }

        Task<int> SaveChangesAsync();
    }
}
