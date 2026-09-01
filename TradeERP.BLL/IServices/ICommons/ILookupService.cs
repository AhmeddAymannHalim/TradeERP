using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.IServices.ICommons
{
    public interface ILookupService
    {
        Task<List<LookupItem>> SpecializationLookupAsync();
        Task<List<LookupItem>> EmployeeLookupAsync();
        Task<List<LookupItem>> CountryLookupAsync();
        Task<List<LookupItem>> GovLookupByCountryIdAsync(int countryId);
        Task<List<LookupItem>> TownLookupByGovIdAsync(int govId);
        Task<List<LookupItem>> VillageLookupByTownIdAsync(int townId);
    }
}
