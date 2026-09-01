using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Commons
{
    public class LookupRepository : ILookupRepository
    {
        private readonly ApplicationDbContext _context;

        public LookupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LookupItem>> SpecializationLookupAsync()
        {
            return await _context.Specializations
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> EmployeeLookupAsync()
        {
            return await _context.Employees
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> CountryLookupAsync()
        {
            return await _context.Countries
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> GovLookupByCountryIdAsync(int countryId)
        {
            return await _context.Governorates
                .Where(g => g.CountryId == countryId)
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> TownLookupByGovIdAsync(int govId)
        {
            return await _context.Towns
                .Where(t => t.GovernorateId == govId)
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> VillageLookupByTownIdAsync(int townId)
        {
            return await _context.Villages
                .Where(v => v.TownId == townId)
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
