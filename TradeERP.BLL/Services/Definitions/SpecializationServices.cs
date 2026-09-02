using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class SpecializationServices : ISpecializationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecializationServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<SpecializationViewModel>> GetPagedSpecializations(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.Specializations.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<SpecializationViewModel>>(result);
        }

        public async Task<SpecializationViewModel?> GetSpecializationById(int id)
        {
            var model = await _unitOfWork.Specializations.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<SpecializationViewModel>(model);
        }

        public async Task<int> GetNewSpecializationCodeAsync()
        {
            return await _unitOfWork.Specializations.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddSpecialization(SpecializationViewModel viewModel)
        {
            var model = _mapper.Map<Specialization>(viewModel);
            return await _unitOfWork.Specializations.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateSpecialization(SpecializationViewModel viewModel)
        {
            var model = _mapper.Map<Specialization>(viewModel);
            return await _unitOfWork.Specializations.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteSpecialization(int id)
        {
            return await _unitOfWork.Specializations.DeleteAsync(id);
        }
    }
}
