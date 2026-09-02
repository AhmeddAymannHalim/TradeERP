using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class EntrySettingServices : IEntrySettingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntrySettingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<EntrySettingViewModel>> GetPagedEntrySettings(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.EntrySettings.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<EntrySettingViewModel>>(result);
        }

        public async Task<EntrySettingViewModel?> GetEntrySettingById(int id)
        {
            var model = await _unitOfWork.EntrySettings.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<EntrySettingViewModel>(model);
        }

        public async Task<ResultMessage> AddEntrySetting(EntrySettingViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntrySetting>(viewModel);
            return await _unitOfWork.EntrySettings.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateEntrySetting(EntrySettingViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.EntrySetting>(viewModel);
            return await _unitOfWork.EntrySettings.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteEntrySetting(int id)
        {
            return await _unitOfWork.EntrySettings.DeleteAsync(id);
        }
    }
}
