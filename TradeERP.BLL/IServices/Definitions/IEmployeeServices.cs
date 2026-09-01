using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface IEmployeeServices
    {
        Task<PaginatedResult<EmployeeViewModel>> GetPagedEmployees(int pageNo, string? searchString);
        Task<EmployeeViewModel?> GetEmployeeById(int id);
        Task<int> GetNewEmployeeCodeAsync();
        Task<ResultMessage> AddEmployee(EmployeeViewModel viewModel);
        Task<ResultMessage> UpdateEmployee(EmployeeViewModel viewModel);
        Task<ResultMessage> DeleteEmployee(int id);
    }
}
