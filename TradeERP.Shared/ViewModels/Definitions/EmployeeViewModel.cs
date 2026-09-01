using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
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

        public int? SpecializationId { get; set; }
        public List<LookupItem> Specializations { get; set; } = new List<LookupItem>();

        public int? CountryId { get; set; }
        public int? GovId { get; set; }
        public int? TownId { get; set; }
        public int? VillageId { get; set; }

        public List<LookupItem> Countries { get; set; } = new List<LookupItem>();
        public List<LookupItem> Govs { get; set; } = new List<LookupItem>();
        public List<LookupItem> Towns { get; set; } = new List<LookupItem>();
        public List<LookupItem> Villages { get; set; } = new List<LookupItem>();
    }
}
