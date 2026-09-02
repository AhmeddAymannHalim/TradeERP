using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<EmployeeViewModel>> GetPagedEmployees(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Employees.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<EmployeeViewModel>>(result);
        }

        public async Task<EmployeeViewModel?> GetEmployeeById(int id)
        {
            var model = await _unitOfWork.Employees.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<EmployeeViewModel>(model);
        }

        public async Task<int> GetNewEmployeeCodeAsync()
        {
            return await _unitOfWork.Employees.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddEmployee(EmployeeViewModel viewModel)
        {
            var model = _mapper.Map<Employee>(viewModel);
            return await _unitOfWork.Employees.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateEmployee(EmployeeViewModel viewModel)
        {
            var model = _mapper.Map<Employee>(viewModel);
            return await _unitOfWork.Employees.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteEmployee(int id)
        {
            return await _unitOfWork.Employees.DeleteAsync(id);
        }
    }
}
