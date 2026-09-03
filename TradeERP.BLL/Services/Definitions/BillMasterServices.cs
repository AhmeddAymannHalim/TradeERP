using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class BillMasterServices : IBillMasterServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillMasterServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BillMasterViewModel>> GetPagedBillMasters(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.BillMasters.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<BillMasterViewModel>>(result);
        }

        public async Task<BillMasterViewModel?> GetBillMasterById(int id)
        {
            var model = await _unitOfWork.BillMasters.GetByIdWithDetailsAsync(id);
            return model == null ? null : _mapper.Map<BillMasterViewModel>(model);
        }

        public async Task<ResultMessage> AddBillMaster(BillMasterViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillMaster>(viewModel);
            var lines = _mapper.Map<List<TradeERP.DAL.Models.BillDetails>>(viewModel.Lines);
            return await _unitOfWork.BillMasters.AddWithDetailsAndPostAsync(model, lines);
        }

        public async Task<ResultMessage> UpdateBillMaster(BillMasterViewModel viewModel)
        {
            var model = _mapper.Map<TradeERP.DAL.Models.BillMaster>(viewModel);
            var lines = _mapper.Map<List<TradeERP.DAL.Models.BillDetails>>(viewModel.Lines);
            return await _unitOfWork.BillMasters.UpdateWithDetailsAsync(model, lines);
        }

        public async Task<ResultMessage> DeleteBillMaster(int id)
        {
            return await _unitOfWork.BillMasters.DeleteAsync(id);
        }

        public async Task<ResultMessage> PostBillMaster(int id)
        {
            return await _unitOfWork.BillMasters.PostBillAsync(id);
        }

        public async Task<string> GetNewBillMasterCodeAsync()
        {
            return await _unitOfWork.BillMasters.GetNewCodeAsync();
        }

        public async Task<JournalEntryViewModel?> GetJournalEntryForBill(int billId)
        {
            var entry = await _unitOfWork.BillMasters.GetJournalEntryAsync(billId);
            return entry == null ? null : _mapper.Map<JournalEntryViewModel>(entry);
        }
    }
}
