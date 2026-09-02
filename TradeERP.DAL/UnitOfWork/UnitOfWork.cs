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
        private IEmployeeRepository? _employees;
        private ISpecializationRepository? _specializations;
        private IDepartmentRepository? _departments;
        private ILookupRepository? _lookups;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEmployeeRepository Employees
            => _employees ??= new EmployeeRepository(_context);

        public ISpecializationRepository Specializations
            => _specializations ??= new SpecializationRepository(_context);

        public IDepartmentRepository Departments
            => _departments ??= new DepartmentRepository(_context);

        public ILookupRepository Lookups
            => _lookups ??= new LookupRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
