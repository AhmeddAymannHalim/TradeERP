namespace TradeERP.DAL.Models
{
    public class Department : BaseEntity, ICodeDefinition
    {
        public string Code { get; set; } = null!;
        public string ArName { get; set; } = null!;
        public string EnName { get; set; } = null!;
        public bool HasShiftNight { get; set; }
        public bool IsActive { get; set; } = true;

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }
        public ICollection<Department> SubDepartments { get; set; } = new List<Department>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
