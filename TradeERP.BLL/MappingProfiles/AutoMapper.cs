using AutoMapper;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.MappingProfiles
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));

            CreateMap<Employee, EmployeeViewModel>()
                .ReverseMap()
                .ForMember(d => d.Specialization, o => o.Ignore())
                .ForMember(d => d.Department, o => o.Ignore())
                .ForMember(d => d.Manager, o => o.Ignore())
                .ForMember(d => d.Subordinates, o => o.Ignore())
                .ForMember(d => d.Nationality, o => o.Ignore())
                .ForMember(d => d.Country, o => o.Ignore())
                .ForMember(d => d.Governorate, o => o.Ignore())
                .ForMember(d => d.Town, o => o.Ignore())
                .ForMember(d => d.Village, o => o.Ignore());

            CreateMap<Specialization, SpecializationViewModel>()
                .ReverseMap();

            CreateMap<Department, DepartmentViewModel>()
                .ReverseMap()
                .ForMember(d => d.Employees, o => o.Ignore())
                .ForMember(d => d.Manager, o => o.Ignore())
                .ForMember(d => d.ParentDepartment, o => o.Ignore())
                .ForMember(d => d.SubDepartments, o => o.Ignore());

            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(d => d.Category, o => o.Ignore());

            CreateMap<Customer, CustomerViewModel>()
                .ReverseMap()
                .ForMember(d => d.LedgerAccount, o => o.Ignore());

            CreateMap<Supplier, SupplierViewModel>()
                .ReverseMap()
                .ForMember(d => d.LedgerAccount, o => o.Ignore());

            CreateMap<LedgerAccount, LedgerAccountViewModel>()
                .ReverseMap();

            CreateMap<BillMaster, BillMasterViewModel>()
                .ForMember(d => d.Lines, o => o.MapFrom(s => s.BillDetails))
                 .ForMember(d => d.CustomerArName, o => o.MapFrom(s => s.Customer.ArName ?? ""))
                .ForMember(d => d.CustomerEnName, o => o.MapFrom(s => s.Customer.EnName ?? ""))
                .ReverseMap()
                .ForMember(d => d.Customer, o => o.Ignore())
                .ForMember(d => d.Supplier, o => o.Ignore())
                .ForMember(d => d.BillDetails, o => o.Ignore());

            CreateMap<BillDetails, BillMasterLineViewModel>()
                .ReverseMap()
                .ForMember(d => d.BillMaster, o => o.Ignore())
                .ForMember(d => d.Product, o => o.Ignore())
                .ForMember(d => d.Code, o => o.Ignore())
                .ForMember(d => d.BillMasterId, o => o.Ignore());

            CreateMap<BillDetails, BillDetailsViewModel>()
                .ForMember(d => d.BillMasterCode, o => o.MapFrom(s => s.BillMaster != null ? s.BillMaster.Code : string.Empty))
                .ForMember(d => d.ProductArName, o => o.MapFrom(s => s.Product != null ? s.Product.ArName : string.Empty))
                .ForMember(d => d.ProductEnName, o => o.MapFrom(s => s.Product != null ? s.Product.EnName : string.Empty))
                .ReverseMap()
                .ForMember(d => d.BillMaster, o => o.Ignore())
                .ForMember(d => d.Product, o => o.Ignore());

            CreateMap<EntryMaster, EntryMasterViewModel>()
                .ForMember(d => d.Lines, o => o.MapFrom(s => s.EntryDetails))
                .ReverseMap()
                .ForMember(d => d.EntryDetails, o => o.Ignore())
                .ForMember(d => d.SourceBillMaster, o => o.Ignore())
                .ForMember(d => d.SourceVoucherMaster, o => o.Ignore());

            CreateMap<EntryDetails, EntryLineViewModel>()
                .ReverseMap()
                .ForMember(d => d.EntryMaster, o => o.Ignore())
                .ForMember(d => d.LedgerAccount, o => o.Ignore())
                .ForMember(d => d.Code, o => o.Ignore())
                .ForMember(d => d.EntryMasterId, o => o.Ignore());

            CreateMap<EntryMaster, JournalEntryViewModel>()
                .ForMember(d => d.Lines, o => o.MapFrom(s => s.EntryDetails));

            CreateMap<EntryDetails, JournalEntryLineViewModel>()
                .ForMember(d => d.LedgerAccountArName, o => o.MapFrom(s => s.LedgerAccount.ArName))
                .ForMember(d => d.LedgerAccountEnName, o => o.MapFrom(s => s.LedgerAccount.EnName));

            CreateMap<EntryDetails, EntryDetailsViewModel>()
                .ForMember(d => d.EntryMasterCode, o => o.MapFrom(s => s.EntryMaster != null ? s.EntryMaster.Code : string.Empty))
                .ForMember(d => d.LedgerAccountArName, o => o.MapFrom(s => s.LedgerAccount != null ? s.LedgerAccount.ArName : string.Empty))
                .ForMember(d => d.LedgerAccountEnName, o => o.MapFrom(s => s.LedgerAccount != null ? s.LedgerAccount.EnName : string.Empty))
                .ReverseMap()
                .ForMember(d => d.EntryMaster, o => o.Ignore())
                .ForMember(d => d.LedgerAccount, o => o.Ignore());

            CreateMap<VoucherMaster, VoucherMasterViewModel>()
                .ReverseMap()
                .ForMember(d => d.Customer, o => o.Ignore())
                .ForMember(d => d.Supplier, o => o.Ignore())
                .ForMember(d => d.TreasuryLedgerAccount, o => o.Ignore());

            CreateMap<AccountingPeriod, AccountingPeriodViewModel>()
                .ReverseMap();

            CreateMap<StockLedger, StockLedgerViewModel>()
                .ForMember(d => d.ProductArName, o => o.MapFrom(s => s.Product.ArName))
                .ForMember(d => d.ProductEnName, o => o.MapFrom(s => s.Product.EnName));

            CreateMap<BillSetting, BillSettingViewModel>()
                .ReverseMap();

            CreateMap<EntrySetting, EntrySettingViewModel>()
                .ReverseMap();
        }
    }
}
