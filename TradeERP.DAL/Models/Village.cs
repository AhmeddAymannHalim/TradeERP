namespace TradeERP.DAL.Models
{
    public class Village : BaseEntity
    {
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;

        public int TownId { get; set; }
        public Town? Town { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
