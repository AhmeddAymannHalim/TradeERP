namespace TradeERP.Shared.ViewModels.Commons
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalRecords { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int NoOfPages { get; set; }
        public string? SearchString { get; set; }
    }
}
