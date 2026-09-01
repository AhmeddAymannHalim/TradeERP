namespace TradeERP.DAL.Models
{
    public class Governorate : BaseEntity
    {
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public ICollection<Town> Towns { get; set; } = new List<Town>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
