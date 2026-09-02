using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class BillSettingServices : IBillSettingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillSettingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BillSettingViewModel>> GetPagedBillSettings(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.BillSettings.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<BillSettingViewModel>>(result);
        }

        public async Task<BillSettingViewModel?> GetBillSettingById(int id)
        {
            var model = await _unitOfWork.BillSettings.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<BillSettingViewModel>(model);
        }

        public async Task<ResultMessage> AddBillSetting(BillSettingViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillSetting>(viewModel);
            return await _unitOfWork.BillSettings.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateBillSetting(BillSettingViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillSetting>(viewModel);
            return await _unitOfWork.BillSettings.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteBillSetting(int id)
        {
            return await _unitOfWork.BillSettings.DeleteAsync(id);
        }
    }
}
