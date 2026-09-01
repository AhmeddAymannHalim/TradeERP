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
            var result = await _unitOfWork.Definitions.GetPagedEmployees(pageNo, searchString);

            return new PaginatedResult<EmployeeViewModel>
            {
                Data = _mapper.Map<List<EmployeeViewModel>>(result.Data),
                TotalRecords = result.TotalRecords,
                PageNo = result.PageNo,
                PageSize = result.PageSize,
                NoOfPages = result.NoOfPages,
                SearchString = result.SearchString
            };
        }

        public async Task<EmployeeViewModel?> GetEmployeeById(int id)
        {
            var model = await _unitOfWork.Definitions.GetEmployeeById(id);
            return model == null ? null : _mapper.Map<EmployeeViewModel>(model);
        }

        public async Task<int> GetNewEmployeeCodeAsync()
        {
            return await _unitOfWork.Definitions.GetNewEmployeeCodeAsync();
        }

        public async Task<ResultMessage> AddEmployee(EmployeeViewModel viewModel)
        {
            var model = _mapper.Map<Employee>(viewModel);
            return await _unitOfWork.Definitions.AddEmployee(model);
        }

        public async Task<ResultMessage> UpdateEmployee(EmployeeViewModel viewModel)
        {
            var model = _mapper.Map<Employee>(viewModel);
            return await _unitOfWork.Definitions.UpdateEmployee(model);
        }

        public async Task<ResultMessage> DeleteEmployee(int id)
        {
            return await _unitOfWork.Definitions.DeleteEmployee(id);
        }
    }
}
