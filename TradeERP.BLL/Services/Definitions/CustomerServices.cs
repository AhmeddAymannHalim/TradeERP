using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CustomerViewModel>> GetPagedCustomers(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Customers.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<CustomerViewModel>>(result);
        }

        public async Task<CustomerViewModel?> GetCustomerById(int id)
        {
            var model = await _unitOfWork.Customers.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<CustomerViewModel>(model);
        }

        public async Task<int> GetNewCustomerCodeAsync()
        {
            return await _unitOfWork.Customers.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddCustomer(CustomerViewModel viewModel)
        {
            var model = _mapper.Map<Customer>(viewModel);
            return await _unitOfWork.Customers.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateCustomer(CustomerViewModel viewModel)
        {
            var model = _mapper.Map<Customer>(viewModel);
            return await _unitOfWork.Customers.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteCustomer(int id)
        {
            return await _unitOfWork.Customers.DeleteAsync(id);
        }
    }
}
