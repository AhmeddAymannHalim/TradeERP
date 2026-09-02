using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class SupplierServices : ISupplierServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<SupplierViewModel>> GetPagedSuppliers(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Suppliers.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<SupplierViewModel>>(result);
        }

        public async Task<SupplierViewModel?> GetSupplierById(int id)
        {
            var model = await _unitOfWork.Suppliers.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<SupplierViewModel>(model);
        }

        public async Task<int> GetNewSupplierCodeAsync()
        {
            return await _unitOfWork.Suppliers.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddSupplier(SupplierViewModel viewModel)
        {
            var model = _mapper.Map<Supplier>(viewModel);
            return await _unitOfWork.Suppliers.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateSupplier(SupplierViewModel viewModel)
        {
            var model = _mapper.Map<Supplier>(viewModel);
            return await _unitOfWork.Suppliers.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteSupplier(int id)
        {
            return await _unitOfWork.Suppliers.DeleteAsync(id);
        }
    }
}
