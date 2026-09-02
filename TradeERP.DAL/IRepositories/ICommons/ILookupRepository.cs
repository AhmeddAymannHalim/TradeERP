using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.ICommons
{
    public interface ILookupRepository
    {
        Task<IEnumerable<LookupItem>> SpecializationLookupAsync();
        Task<IEnumerable<LookupItem>> EmployeeLookupAsync();
        Task<IEnumerable<LookupItem>> DepartmentLookupAsync();
        Task<IEnumerable<LookupItem>> CountryLookupAsync();
        Task<IEnumerable<LookupItem>> GovLookupByCountryIdAsync(int countryId);
        Task<IEnumerable<LookupItem>> TownLookupByGovIdAsync(int govId);
        Task<IEnumerable<LookupItem>> VillageLookupByTownIdAsync(int townId);
    }
}
