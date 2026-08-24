namespace TradeERP.DAL.IRepositories
{
    // ---------------------------------------------------------------------
    // IDefinitionRepository — combined (NON-generic) repository contract for
    // every "Definitions" module entity (Employee, Department, Category, ...).
    //
    // Naming convention to follow for every future entity added here:
    //
    //   Task<IEnumerable<{Entity}>> GetAll{Entity}sAsync();
    //   Task<{Entity}?> Get{Entity}ByIdAsync(int id);
    //   Task Add{Entity}Async({Entity} entity);
    //   Task Update{Entity}Async({Entity} entity);
    //   Task Delete{Entity}Async(int id);
    //
    // Example (once the Employee entity exists):
    //
    //   Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    //   Task<Employee?> GetEmployeeByIdAsync(int id);
    //   Task AddEmployeeAsync(Employee entity);
    //   Task UpdateEmployeeAsync(Employee entity);
    //   Task DeleteEmployeeAsync(int id);
    //
    // Example (Department, added alongside Employee in the same repository):
    //
    //   Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    //   Task<Department?> GetDepartmentByIdAsync(int id);
    //   Task AddDepartmentAsync(Department entity);
    //   Task UpdateDepartmentAsync(Department entity);
    //   Task DeleteDepartmentAsync(int id);
    //
    // Rules:
    //   - No generic constraints, no reflection, no shared base method.
    //   - Every entity gets its own explicit, fully-typed method set here.
    //   - Reads use AsNoTracking() in the implementation.
    //   - Each entity still gets its own Service / Controller / Views elsewhere
    //     (one-to-one), even though the repository is combined.
    // ---------------------------------------------------------------------
    public interface IDefinitionRepository
    {
        // Entity method groups will be added here as each Definitions module is implemented.
    }
}
