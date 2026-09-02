using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IProductServices
    {
        Task<PaginatedResult<ProductViewModel>> GetPagedProducts(int pageNo, string? searchString);
        Task<ProductViewModel?> GetProductById(int id);
        Task<int> GetNewProductCodeAsync();
        Task<ResultMessage> AddProduct(ProductViewModel viewModel);
        Task<ResultMessage> UpdateProduct(ProductViewModel viewModel);
        Task<ResultMessage> DeleteProduct(int id);
    }
}
