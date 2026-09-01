using AutoMapper;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.MappingProfiles
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ReverseMap()
                .ForMember(d => d.Specialization, o => o.Ignore())
                .ForMember(d => d.Country, o => o.Ignore())
                .ForMember(d => d.Governorate, o => o.Ignore())
                .ForMember(d => d.Town, o => o.Ignore())
                .ForMember(d => d.Village, o => o.Ignore());

            CreateMap<Specialization, SpecializationViewModel>()
                .ReverseMap();
        }
    }
}
