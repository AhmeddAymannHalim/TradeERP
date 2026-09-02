using TradeERP.BLL.IServices.ICommons;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Services.Commons
{
    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LookupItem>> SpecializationLookupAsync()
            => (await _unitOfWork.Lookups.SpecializationLookupAsync()).ToList();

        public async Task<List<LookupItem>> EmployeeLookupAsync()
            => (await _unitOfWork.Lookups.EmployeeLookupAsync()).ToList();

        public async Task<List<LookupItem>> DepartmentLookupAsync()
            => (await _unitOfWork.Lookups.DepartmentLookupAsync()).ToList();

        public async Task<List<LookupItem>> CountryLookupAsync()
            => (await _unitOfWork.Lookups.CountryLookupAsync()).ToList();

        public async Task<List<LookupItem>> GovLookupByCountryIdAsync(int countryId)
            => (await _unitOfWork.Lookups.GovLookupByCountryIdAsync(countryId)).ToList();

        public async Task<List<LookupItem>> TownLookupByGovIdAsync(int govId)
            => (await _unitOfWork.Lookups.TownLookupByGovIdAsync(govId)).ToList();

        public async Task<List<LookupItem>> VillageLookupByTownIdAsync(int townId)
            => (await _unitOfWork.Lookups.VillageLookupByTownIdAsync(townId)).ToList();

        public async Task<List<LookupItem>> CategoryLookupAsync()
            => (await _unitOfWork.Lookups.CategoryLookupAsync()).ToList();

        public async Task<List<LookupItem>> ProductLookupAsync()
            => (await _unitOfWork.Lookups.ProductLookupAsync()).ToList();

        public async Task<List<LookupItem>> CustomerLookupAsync()
            => (await _unitOfWork.Lookups.CustomerLookupAsync()).ToList();

        public async Task<List<LookupItem>> SupplierLookupAsync()
            => (await _unitOfWork.Lookups.SupplierLookupAsync()).ToList();

        public async Task<List<LookupItem>> LedgerAccountLookupAsync()
            => (await _unitOfWork.Lookups.LedgerAccountLookupAsync()).ToList();

        public async Task<List<LookupItem>> BillMasterLookupAsync()
            => (await _unitOfWork.Lookups.BillMasterLookupAsync()).ToList();

        public async Task<List<LookupItem>> EntryMasterLookupAsync()
            => (await _unitOfWork.Lookups.EntryMasterLookupAsync()).ToList();
    }
}
