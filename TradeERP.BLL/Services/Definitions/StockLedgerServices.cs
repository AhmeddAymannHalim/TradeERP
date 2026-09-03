using AutoMapper;
using TradeERP.BLL.IServices.Definitions;
using TradeERP.DAL.UnitOfWork;
using TradeERP.Shared.ViewModels.Commons;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Services.Definitions
{
    public class StockLedgerServices : IStockLedgerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockLedgerServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<StockLedgerViewModel>> GetPagedStockLedger(int pageNo, string? searchString)
        {
            var result = await _unitOfWork.StockLedgers.GetPagedAsync(pageNo, searchString);
            return _mapper.Map<PaginatedResult<StockLedgerViewModel>>(result);
        }
    }
}
