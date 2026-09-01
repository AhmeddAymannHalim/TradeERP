namespace TradeERP.DAL.Models
{
    public class Specialization : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
