using TradeERP.Shared.Enums;

namespace TradeERP.DAL.Models
{
    public class Employee : BaseEntity, ICodeDefinition
    {
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; } = true;

        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public ContractType ContractType { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public string InsuranceNumber { get; set; } = string.Empty;
        public string EmployeeImage { get; set; } = string.Empty;
        public DateTime? TerminationDate { get; set; }

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

        public int? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? NationalityId { get; set; }
        public Country? Nationality { get; set; }

        public int? CountryId { get; set; }
        public Country? Country { get; set; }

        public int? GovId { get; set; }
        public Governorate? Governorate { get; set; }

        public int? TownId { get; set; }
        public Town? Town { get; set; }

        public int? VillageId { get; set; }
        public Village? Village { get; set; }
    }
}
