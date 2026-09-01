using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.IServices.Definitions
{
    public interface ISpecializationServices
    {
        Task<PaginatedResult<SpecializationViewModel>> GetPagedSpecializations(int pageNo, string? searchString);
        Task<SpecializationViewModel?> GetSpecializationById(int id);
        Task<int> GetNewSpecializationCodeAsync();
        Task<ResultMessage> AddSpecialization(SpecializationViewModel viewModel);
        Task<ResultMessage> UpdateSpecialization(SpecializationViewModel viewModel);
        Task<ResultMessage> DeleteSpecialization(int id);
    }
}
