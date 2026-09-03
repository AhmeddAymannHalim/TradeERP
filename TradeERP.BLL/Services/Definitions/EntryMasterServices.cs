using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class EntryMasterServices : IEntryMasterServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntryMasterServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<EntryMasterViewModel>> GetPagedEntryMasters(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.EntryMasters.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<EntryMasterViewModel>>(result);
        }

        public async Task<EntryMasterViewModel?> GetEntryMasterById(int id)
        {
            var model = await _unitOfWork.EntryMasters.GetByIdWithDetailsAsync(id);
            return model == null ? null : _mapper.Map<EntryMasterViewModel>(model);
        }

        public async Task<ResultMessage> AddEntryMaster(EntryMasterViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntryMaster>(viewModel);
            var lines = _mapper.Map<List<TradeERP.DAL.Models.EntryDetails>>(viewModel.Lines);
            return await _unitOfWork.EntryMasters.AddWithLinesAsync(model, lines);
        }

        public async Task<ResultMessage> UpdateEntryMaster(EntryMasterViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntryMaster>(viewModel);
            var lines = _mapper.Map<List<TradeERP.DAL.Models.EntryDetails>>(viewModel.Lines);
            return await _unitOfWork.EntryMasters.UpdateWithLinesAsync(model, lines);
        }

        public async Task<ResultMessage> DeleteEntryMaster(int id)
        {
            return await _unitOfWork.EntryMasters.DeleteAsync(id);
        }

        public async Task<string> GetNewEntryMasterCodeAsync()
        {
            return await _unitOfWork.EntryMasters.GetNewCodeAsync();
        }
    }
}
