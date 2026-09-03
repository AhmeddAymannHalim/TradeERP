using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class VoucherMasterServices : IVoucherMasterServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VoucherMasterServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VoucherMasterViewModel>> GetPagedVoucherMasters(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.VoucherMasters.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<VoucherMasterViewModel>>(result);
        }

        public async Task<VoucherMasterViewModel?> GetVoucherMasterById(int id)
        {
            var model = await _unitOfWork.VoucherMasters.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<VoucherMasterViewModel>(model);
        }

        public async Task<ResultMessage> AddVoucherMaster(VoucherMasterViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.VoucherMaster>(viewModel);
            return await _unitOfWork.VoucherMasters.AddAndPostAsync(model);
        }

        public async Task<ResultMessage> DeleteVoucherMaster(int id)
        {
            return await _unitOfWork.VoucherMasters.DeleteAsync(id);
        }

        public async Task<int> GetNewVoucherMasterCodeAsync()
        {
            return await _unitOfWork.VoucherMasters.GetNewCodeAsync();
        }
    }
}
