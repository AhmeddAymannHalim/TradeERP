using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.IServices.ICommons
{
    public interface ILookupService
    {
        Task<List<LookupItem>> SpecializationLookupAsync();
        Task<List<LookupItem>> EmployeeLookupAsync();
        Task<List<LookupItem>> DepartmentLookupAsync();
        Task<List<LookupItem>> CountryLookupAsync();
        Task<List<LookupItem>> GovLookupByCountryIdAsync(int countryId);
        Task<List<LookupItem>> TownLookupByGovIdAsync(int govId);
        Task<List<LookupItem>> VillageLookupByTownIdAsync(int townId);
        Task<List<LookupItem>> CategoryLookupAsync();
        Task<List<LookupItem>> ProductLookupAsync();
        Task<List<LookupItem>> CustomerLookupAsync();
        Task<List<LookupItem>> SupplierLookupAsync();
        Task<List<LookupItem>> LedgerAccountLookupAsync();
        Task<List<LookupItem>> BillMasterLookupAsync();
        Task<List<LookupItem>> EntryMasterLookupAsync();
    }
}
