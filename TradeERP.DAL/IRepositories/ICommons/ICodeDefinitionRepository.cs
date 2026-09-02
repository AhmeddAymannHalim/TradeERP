using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.ICommons
{
    public interface ICodeDefinitionRepository<T> : IGenericRepository<T> where T : BaseEntity, ICodeDefinition
    {
        Task<PaginatedResult<T>> GetPagedAsync(int pageNo, string? searchString);
        Task<int> GetNewCodeAsync();
    }
}
