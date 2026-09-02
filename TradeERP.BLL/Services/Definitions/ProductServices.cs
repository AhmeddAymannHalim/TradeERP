using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ProductViewModel>> GetPagedProducts(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Products.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<ProductViewModel>>(result);
        }

        public async Task<ProductViewModel?> GetProductById(int id)
        {
            var model = await _unitOfWork.Products.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<ProductViewModel>(model);
        }

        public async Task<int> GetNewProductCodeAsync()
        {
            return await _unitOfWork.Products.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddProduct(ProductViewModel viewModel)
        {
            var model = _mapper.Map<Product>(viewModel);
            return await _unitOfWork.Products.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateProduct(ProductViewModel viewModel)
        {
            var model = _mapper.Map<Product>(viewModel);
            return await _unitOfWork.Products.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteProduct(int id)
        {
            return await _unitOfWork.Products.DeleteAsync(id);
        }
    }
}
