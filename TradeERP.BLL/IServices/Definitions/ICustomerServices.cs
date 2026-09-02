using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface ICustomerServices
    {
        Task<PaginatedResult<CustomerViewModel>> GetPagedCustomers(int pageNo, string? searchString);
        Task<CustomerViewModel?> GetCustomerById(int id);
        Task<int> GetNewCustomerCodeAsync();
        Task<ResultMessage> AddCustomer(CustomerViewModel viewModel);
        Task<ResultMessage> UpdateCustomer(CustomerViewModel viewModel);
        Task<ResultMessage> DeleteCustomer(int id);
    }
}
