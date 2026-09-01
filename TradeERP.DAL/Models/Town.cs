namespace TradeERP.DAL.Models
{
    public class Town : BaseEntity
    {
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public int GovernorateId { get; set; }
        public Governorate? Governorate { get; set; }

        public ICollection<Village> Villages { get; set; } = new List<Village>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
