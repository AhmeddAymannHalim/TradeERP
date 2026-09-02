using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface ISupplierServices
    {
        Task<PaginatedResult<SupplierViewModel>> GetPagedSuppliers(int pageNo, string? searchString);
        Task<SupplierViewModel?> GetSupplierById(int id);
        Task<int> GetNewSupplierCodeAsync();
        Task<ResultMessage> AddSupplier(SupplierViewModel viewModel);
        Task<ResultMessage> UpdateSupplier(SupplierViewModel viewModel);
        Task<ResultMessage> DeleteSupplier(int id);
    }
}
