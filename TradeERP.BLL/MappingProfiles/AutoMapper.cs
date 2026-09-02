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
        }
    }
}
