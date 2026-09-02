using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class LedgerAccountServices : ILedgerAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LedgerAccountServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<LedgerAccountViewModel>> GetPagedLedgerAccounts(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.LedgerAccounts.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<LedgerAccountViewModel>>(result);
        }

        public async Task<LedgerAccountViewModel?> GetLedgerAccountById(int id)
        {
            var model = await _unitOfWork.LedgerAccounts.GetByIdAsync(id);
            return model == null ? null : _mapper.Map<LedgerAccountViewModel>(model);
        }

        public async Task<int> GetNewLedgerAccountCodeAsync()
        {
            return await _unitOfWork.LedgerAccounts.GetNewCodeAsync();
        }

        public async Task<ResultMessage> AddLedgerAccount(LedgerAccountViewModel viewModel)
        {
            var model = _mapper.Map<LedgerAccount>(viewModel);
            return await _unitOfWork.LedgerAccounts.AddAsync(model);
        }

        public async Task<ResultMessage> UpdateLedgerAccount(LedgerAccountViewModel viewModel)
        {
            var model = _mapper.Map<LedgerAccount>(viewModel);
            return await _unitOfWork.LedgerAccounts.UpdateAsync(model);
        }

        public async Task<ResultMessage> DeleteLedgerAccount(int id)
        {
            return await _unitOfWork.LedgerAccounts.DeleteAsync(id);
        }
    }
}
