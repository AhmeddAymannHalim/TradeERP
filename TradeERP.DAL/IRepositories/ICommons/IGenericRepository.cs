using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.ICommons
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<ResultMessage> AddAsync(T entity);
        Task<ResultMessage> UpdateAsync(T entity);
        Task<ResultMessage> DeleteAsync(int id);
    }
}
