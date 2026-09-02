using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.ViewModels.Definitions
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string ArName { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public bool HasShiftNight { get; set; }
        public bool IsActive { get; set; } = true;

        public int? ManagerId { get; set; }
        public List<LookupItem> Managers { get; set; } = new List<LookupItem>();

        public int? ParentDepartmentId { get; set; }
        public List<LookupItem> ParentDepartments { get; set; } = new List<LookupItem>();
    }
}
