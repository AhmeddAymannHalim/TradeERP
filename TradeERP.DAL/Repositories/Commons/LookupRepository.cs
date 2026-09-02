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
                    ArName = a.ArName + "-" + a.Code,
                    EnName = a.EnName + "-" + a.Code
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> DepartmentLookupAsync()
        {
            return await _context.Departments
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

        public async Task<IEnumerable<LookupItem>> CategoryLookupAsync()
        {
            return await _context.Categories
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> ProductLookupAsync()
        {
            return await _context.Products
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> CustomerLookupAsync()
        {
            return await _context.Customers
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> SupplierLookupAsync()
        {
            return await _context.Suppliers
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> LedgerAccountLookupAsync()
        {
            return await _context.LedgerAccounts
                .Select(a => new LookupItem
                {
                    Id = a.Id,
                    ArName = a.ArName,
                    EnName = a.EnName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // BillMaster/EntryMaster have no ArName/EnName, so Code is projected into both
        // LookupItem display fields as a pragmatic reuse of the shared lookup shape.
        public async Task<IEnumerable<LookupItem>> BillMasterLookupAsync()
        {
            return await _context.BillMasters
                .Select(b => new LookupItem
                {
                    Id = b.Id,
                    ArName = b.Code,
                    EnName = b.Code
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> EntryMasterLookupAsync()
        {
            return await _context.EntryMasters
                .Select(m => new LookupItem
                {
                    Id = m.Id,
                    ArName = m.Code,
                    EnName = m.Code
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
