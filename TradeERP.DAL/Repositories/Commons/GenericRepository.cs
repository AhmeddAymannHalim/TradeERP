using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Commons
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<ResultMessage> AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public virtual async Task<ResultMessage> UpdateAsync(T entity)
        {
            try
            {
                var existing = await _context.Set<T>().FindAsync(entity.Id);
                if (existing == null)
                    return new ResultMessage { Success = false, Message = "RecordNotFound" };

                var createdBy = existing.CreatedBy;
                var createdAt = existing.CreatedAt;
                var deletedBy = existing.DeletedBy;
                var deletedAt = existing.DeletedAt;
                var isDeleted = existing.IsDeleted;

                _context.Entry(existing).CurrentValues.SetValues(entity);

                existing.CreatedBy = createdBy;
                existing.CreatedAt = createdAt;
                existing.DeletedBy = deletedBy;
                existing.DeletedAt = deletedAt;
                existing.IsDeleted = isDeleted;

                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public virtual async Task<ResultMessage> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }
    }
}
