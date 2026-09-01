namespace TradeERP.DAL.Models
{
    public class Country : BaseEntity
    {
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public ICollection<Governorate> Governorates { get; set; } = new List<Governorate>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
