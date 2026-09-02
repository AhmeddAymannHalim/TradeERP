using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<DepartmentViewModel>> GetPagedDepartments(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Departments.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<DepartmentViewModel>>(result);
        }

        public async Task<DepartmentViewModel?> GetDepartmentById(int id)
        {
            var model = await _unitOfWork.Departments.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<DepartmentViewModel>(model);
        }

        public async Task<int> GetNewDepartmentCodeAsync()
        {
            return await _unitOfWork.Departments.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddDepartment(DepartmentViewModel viewModel)
        {
            var model = _mapper.Map<Department>(viewModel);
            return await _unitOfWork.Departments.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateDepartment(DepartmentViewModel viewModel)
        {
            var model = _mapper.Map<Department>(viewModel);
            return await _unitOfWork.Departments.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteDepartment(int id)
        {
            return await _unitOfWork.Departments.DeleteAsync(id);
        }
    }
}
