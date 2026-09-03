using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class AccountingPeriodServices : IAccountingPeriodServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountingPeriodServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<AccountingPeriodViewModel>> GetPagedAccountingPeriods(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.AccountingPeriods.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<AccountingPeriodViewModel>>(result);
        }

        public async Task<ResultMessage> AddAccountingPeriod(AccountingPeriodViewModel viewModel)
        {
            var model = _mapper.Map<DAL.Models.AccountingPeriod>(viewModel);
            return await _unitOfWork.AccountingPeriods.AddAsync(model);
        }

        public async Task<ResultMessage> CloseAccountingPeriod(int id)
        {
            return await _unitOfWork.AccountingPeriods.CloseAsync(id);
        }
    }
}
