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

        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        ISupplierRepository Suppliers { get; }
        ILedgerAccountRepository LedgerAccounts { get; }
        IBillMasterRepository BillMasters { get; }
        IBillDetailsRepository BillDetails { get; }
        IEntryMasterRepository EntryMasters { get; }
        IEntryDetailsRepository EntryDetails { get; }
        IBillSettingRepository BillSettings { get; }
        IEntrySettingRepository EntrySettings { get; }

        Task<int> SaveChangesAsync();
    }
}
