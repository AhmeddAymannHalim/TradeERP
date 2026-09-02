using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CategoryViewModel>> GetPagedCategories(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Categories.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<CategoryViewModel>>(result);
        }

        public async Task<CategoryViewModel?> GetCategoryById(int id)
        {
            var model = await _unitOfWork.Categories.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<CategoryViewModel>(model);
        }

        public async Task<int> GetNewCategoryCodeAsync()
        {
            return await _unitOfWork.Categories.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddCategory(CategoryViewModel viewModel)
        {
            var model = _mapper.Map<Category>(viewModel);
            return await _unitOfWork.Categories.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateCategory(CategoryViewModel viewModel)
        {
            var model = _mapper.Map<Category>(viewModel);
            return await _unitOfWork.Categories.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteCategory(int id)
        {
            return await _unitOfWork.Categories.DeleteAsync(id);
        }
    }
}
