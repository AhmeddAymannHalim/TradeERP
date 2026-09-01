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

        public async Task<List<LookupItem>> CountryLookupAsync()
            => (await _unitOfWork.Lookups.CountryLookupAsync()).ToList();

        public async Task<List<LookupItem>> GovLookupByCountryIdAsync(int countryId)
            => (await _unitOfWork.Lookups.GovLookupByCountryIdAsync(countryId)).ToList();

        public async Task<List<LookupItem>> TownLookupByGovIdAsync(int govId)
            => (await _unitOfWork.Lookups.TownLookupByGovIdAsync(govId)).ToList();

        public async Task<List<LookupItem>> VillageLookupByTownIdAsync(int townId)
            => (await _unitOfWork.Lookups.VillageLookupByTownIdAsync(townId)).ToList();
    }
}
