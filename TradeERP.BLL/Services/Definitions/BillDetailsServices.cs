using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class BillDetailsServices : IBillDetailsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillDetailsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BillDetailsViewModel>> GetPagedBillDetails(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.BillDetails.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<BillDetailsViewModel>>(result);
        }

        public async Task<BillDetailsViewModel?> GetBillDetailsById(int id)
        {
            var model = await _unitOfWork.BillDetails.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<BillDetailsViewModel>(model);
        }

        public async Task<ResultMessage> AddBillDetails(BillDetailsViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillDetails>(viewModel);
            return await _unitOfWork.BillDetails.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateBillDetails(BillDetailsViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillDetails>(viewModel);
            return await _unitOfWork.BillDetails.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteBillDetails(int id)
        {
            return await _unitOfWork.BillDetails.DeleteAsync(id);
        }
    }
}
