using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private const int PageSize = 10;

        private readonly ApplicationDbContext _context;

        public DefinitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Employee

        public async Task<PaginatedResult<Employee>> GetPagedEmployees(int pageNo, string? searchString)
        {
            var query = _context.Employees.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(a =>
                    a.Code.ToLower().Contains(searchLower) ||
                    a.ArName.ToLower().Contains(searchLower) ||
                    a.EnName.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(a => a.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<Employee>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<int> GetNewEmployeeCodeAsync()
        {
            var codes = await _context.Employees.Select(e => e.Code).ToListAsync();

            return codes
                .Where(c => int.TryParse(c, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        public async Task<ResultMessage> AddEmployee(Employee entity)
        {
            try
            {
                await _context.Employees.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultMessage> UpdateEmployee(Employee entity)
        {
            try
            {
                _context.Employees.Update(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultMessage> DeleteEmployee(int id)
        {
            var model = await GetEmployeeById(id);
            if (model == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            try
            {
                _context.Employees.Remove(model);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        #endregion

        #region Specialization

        public async Task<PaginatedResult<Specialization>> GetPagedSpecializations(int pageNo, string? searchString)
        {
            var query = _context.Specializations.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(a =>
                    a.Code.ToLower().Contains(searchLower) ||
                    a.ArName.ToLower().Contains(searchLower) ||
                    a.EnName.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(a => a.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<Specialization>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }

        public async Task<Specialization?> GetSpecializationById(int id)
        {
            return await _context.Specializations.FindAsync(id);
        }

        public async Task<int> GetNewSpecializationCodeAsync()
        {
            var codes = await _context.Specializations.Select(s => s.Code).ToListAsync();

            return codes
                .Where(c => int.TryParse(c, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        public async Task<ResultMessage> AddSpecialization(Specialization entity)
        {
            try
            {
                await _context.Specializations.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultMessage> UpdateSpecialization(Specialization entity)
        {
            try
            {
                _context.Specializations.Update(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultMessage> DeleteSpecialization(int id)
        {
            var model = await GetSpecializationById(id);
            if (model == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            try
            {
                _context.Specializations.Remove(model);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
