namespace TradeERP.DAL.Models
{
    public class Employee : BaseEntity
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

        public int? SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }

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
