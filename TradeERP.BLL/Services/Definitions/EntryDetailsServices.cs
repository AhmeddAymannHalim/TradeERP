using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class EntryDetailsServices : IEntryDetailsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntryDetailsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<EntryDetailsViewModel>> GetPagedEntryDetails(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.EntryDetails.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<EntryDetailsViewModel>>(result);
        }

        public async Task<EntryDetailsViewModel?> GetEntryDetailsById(int id)
        {
            var model = await _unitOfWork.EntryDetails.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<EntryDetailsViewModel>(model);
        }

        public async Task<ResultMessage> AddEntryDetails(EntryDetailsViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntryDetails>(viewModel);
            return await _unitOfWork.EntryDetails.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateEntryDetails(EntryDetailsViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntryDetails>(viewModel);
            return await _unitOfWork.EntryDetails.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteEntryDetails(int id)
        {
            return await _unitOfWork.EntryDetails.DeleteAsync(id);
        }
    }
}
