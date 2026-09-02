using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IDepartmentServices
    {
        Task<PaginatedResult<DepartmentViewModel>> GetPagedDepartments(int pageNo, string? searchString);
        Task<DepartmentViewModel?> GetDepartmentById(int id);
        Task<int> GetNewDepartmentCodeAsync();
        Task<ResultMessage> AddDepartment(DepartmentViewModel viewModel);
        Task<ResultMessage> UpdateDepartment(DepartmentViewModel viewModel);
        Task<ResultMessage> DeleteDepartment(int id);
    }
}
