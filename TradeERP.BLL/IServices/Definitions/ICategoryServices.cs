using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface ICategoryServices
    {
        Task<PaginatedResult<CategoryViewModel>> GetPagedCategories(int pageNo, string? searchString);
        Task<CategoryViewModel?> GetCategoryById(int id);
        Task<int> GetNewCategoryCodeAsync();
        Task<ResultMessage> AddCategory(CategoryViewModel viewModel);
        Task<ResultMessage> UpdateCategory(CategoryViewModel viewModel);
        Task<ResultMessage> DeleteCategory(int id);
    }
}
