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
        private ICategoryRepository? _categories;
        private IProductRepository? _products;
        private ICustomerRepository? _customers;
        private ISupplierRepository? _suppliers;
        private ILedgerAccountRepository? _ledgerAccounts;
        private IBillMasterRepository? _billMasters;
        private IBillDetailsRepository? _billDetails;
        private IEntryMasterRepository? _entryMasters;
        private IEntryDetailsRepository? _entryDetails;
        private IBillSettingRepository? _billSettings;
        private IEntrySettingRepository? _entrySettings;
        private IVoucherMasterRepository? _voucherMasters;

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

        public ICategoryRepository Categories
            => _categories ??= new CategoryRepository(_context);

        public IProductRepository Products
            => _products ??= new ProductRepository(_context);

        public ICustomerRepository Customers
            => _customers ??= new CustomerRepository(_context);

        public ISupplierRepository Suppliers
            => _suppliers ??= new SupplierRepository(_context);

        public ILedgerAccountRepository LedgerAccounts
            => _ledgerAccounts ??= new LedgerAccountRepository(_context);

        public IBillMasterRepository BillMasters
            => _billMasters ??= new BillMasterRepository(_context);

        public IBillDetailsRepository BillDetails
            => _billDetails ??= new BillDetailsRepository(_context);

        public IEntryMasterRepository EntryMasters
            => _entryMasters ??= new EntryMasterRepository(_context);

        public IEntryDetailsRepository EntryDetails
            => _entryDetails ??= new EntryDetailsRepository(_context);

        public IBillSettingRepository BillSettings
            => _billSettings ??= new BillSettingRepository(_context);

        public IEntrySettingRepository EntrySettings
            => _entrySettings ??= new EntrySettingRepository(_context);

        public IVoucherMasterRepository VoucherMasters
            => _voucherMasters ??= new VoucherMasterRepository(_context);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
