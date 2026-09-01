using TradeERP.DAL.Models;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.IRepositories.Definitions
{
    public interface IDefinitionRepository
    {
        #region Employee
        Task<PaginatedResult<Employee>> GetPagedEmployees(int pageNo, string? searchString);
        Task<Employee?> GetEmployeeById(int id);
        Task<int> GetNewEmployeeCodeAsync();
        Task<ResultMessage> AddEmployee(Employee entity);
        Task<ResultMessage> UpdateEmployee(Employee entity);
        Task<ResultMessage> DeleteEmployee(int id);
        #endregion

        #region Specialization
        Task<PaginatedResult<Specialization>> GetPagedSpecializations(int pageNo, string? searchString);
        Task<Specialization?> GetSpecializationById(int id);
        Task<int> GetNewSpecializationCodeAsync();
        Task<ResultMessage> AddSpecialization(Specialization entity);
        Task<ResultMessage> UpdateSpecialization(Specialization entity);
        Task<ResultMessage> DeleteSpecialization(int id);
        #endregion
    }
}
