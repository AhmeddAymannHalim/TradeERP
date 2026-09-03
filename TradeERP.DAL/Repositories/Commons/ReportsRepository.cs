using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Definitions;
using TradeERP.Shared.ViewModels.Definitions.Reports;

namespace TradeERP.DAL.Repositories.Commons
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrialBalanceRowViewModel>> GetTrialBalanceAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Set<EntryDetails>().AsNoTracking()
                .Include(d => d.LedgerAccount)
                .Include(d => d.EntryMaster)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(d => d.EntryMaster.EntryDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(d => d.EntryMaster.EntryDate <= toDate.Value);

            return await query
                .GroupBy(d => new { d.LedgerAccountId, d.LedgerAccount.ArName, d.LedgerAccount.EnName })
                .Select(g => new TrialBalanceRowViewModel
                {
                    LedgerAccountArName = g.Key.ArName,
                    LedgerAccountEnName = g.Key.EnName,
                    TotalDebit = g.Sum(d => d.DebitAmount),
                    TotalCredit = g.Sum(d => d.CreditAmount)
                })
                .OrderBy(r => r.LedgerAccountEnName)
                .ToListAsync();
        }

        public async Task<List<StatementOfAccountRowViewModel>> GetStatementOfAccountAsync(int ledgerAccountId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Set<EntryDetails>().AsNoTracking()
                .Include(d => d.EntryMaster)
                .Where(d => d.LedgerAccountId == ledgerAccountId)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(d => d.EntryMaster.EntryDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(d => d.EntryMaster.EntryDate <= toDate.Value);

            var rows = await query
                .OrderBy(d => d.EntryMaster.EntryDate)
                .ThenBy(d => d.Id)
                .Select(d => new StatementOfAccountRowViewModel
                {
                    EntryDate = d.EntryMaster.EntryDate,
                    Description = d.EntryMaster.Description,
                    DebitAmount = d.DebitAmount,
                    CreditAmount = d.CreditAmount
                })
                .ToListAsync();

            var runningBalance = 0m;
            foreach (var row in rows)
            {
                runningBalance += row.DebitAmount - row.CreditAmount;
                row.RunningBalance = runningBalance;
            }

            return rows;
        }

        public async Task<List<StockValuationRowViewModel>> GetStockValuationAsync()
        {
            var grouped = await _context.Set<StockLedger>().AsNoTracking()
                .GroupBy(s => new { s.ProductId, s.Product.ArName, s.Product.EnName })
                .Select(g => new
                {
                    g.Key.ArName,
                    g.Key.EnName,
                    InQty = g.Sum(s => s.MovementType == StockMovementType.In ? s.Quantity : 0),
                    InValue = g.Sum(s => s.MovementType == StockMovementType.In ? s.Quantity * s.UnitCost : 0),
                    OutQty = g.Sum(s => s.MovementType == StockMovementType.Out ? s.Quantity : 0),
                    OutValue = g.Sum(s => s.MovementType == StockMovementType.Out ? s.Quantity * s.UnitCost : 0)
                })
                .ToListAsync();

            return grouped
                .Select(g =>
                {
                    var remainingQty = g.InQty - g.OutQty;
                    var remainingValue = g.InValue - g.OutValue;
                    var averageCost = remainingQty > 0 ? remainingValue / remainingQty : 0;

                    return new StockValuationRowViewModel
                    {
                        ProductArName = g.ArName,
                        ProductEnName = g.EnName,
                        Quantity = remainingQty,
                        AverageCost = averageCost,
                        TotalValue = remainingValue
                    };
                })
                .Where(r => r.Quantity != 0)
                .OrderBy(r => r.ProductEnName)
                .ToList();
        }

        public async Task<List<BillMasterViewModel>> GetBillSalesAsync()
        {
            var bills = await _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.BillDetails)
                .ThenInclude(d => d.Product)
                .Include(d => d.Customer)
                .Where(b => b.BillType == BillType.Sales)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();

            return bills.Select(b => new BillMasterViewModel
            {
                Id = b.Id,
                Code = b.Code,
                BillDate = b.BillDate,
                BillType = b.BillType,
                Amount = b.Amount,
                IsPosted = b.IsPosted,
                CustomerId = b.CustomerId,
                CustomerArName = b.Customer?.ArName ?? "",
                CustomerEnName = b.Customer?.EnName ?? "",
                SupplierId = b.SupplierId,
                Lines = b.BillDetails.Select(d => new BillMasterLineViewModel
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    LineTotal = d.LineTotal
                }).ToList()
            }).ToList();

        }

        public async Task<List<BillMasterViewModel>> GetBillSalesReturnAsync()
        {
            var bills = await _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.BillDetails)
                .ThenInclude(d => d.Product)
                .Include(d => d.Customer)
                .Where(b => b.BillType == BillType.SalesReturn)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();

            return bills.Select(b => new BillMasterViewModel
            {
                Id = b.Id,
                Code = b.Code,
                BillDate = b.BillDate,
                BillType = b.BillType,
                Amount = b.Amount,
                IsPosted = b.IsPosted,
                CustomerId = b.CustomerId,
                CustomerArName = b.Customer?.ArName ?? "",
                CustomerEnName = b.Customer?.EnName ?? "",
                SupplierId = b.SupplierId,
                Lines = b.BillDetails.Select(d => new BillMasterLineViewModel
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    LineTotal = d.LineTotal
                }).ToList()
            }).ToList();

        }


        #region Purchases and returnPurchases
        public async Task<List<BillMasterViewModel>> GetBillPurchasesAsync()
        {
            var bills = await _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.BillDetails)
                .ThenInclude(d => d.Product)
                .Include(d => d.Supplier)
                .Where(b => b.BillType == BillType.Purchase)
                .OrderByDescending(b => b.Amount)
                .ToListAsync();

            return bills.Select(b => new BillMasterViewModel
            {
                Id = b.Id,
                Code = b.Code,
                BillDate = b.BillDate,
                BillType = b.BillType,
                Amount = b.Amount,
                IsPosted = b.IsPosted,
                CustomerId = b.CustomerId,
                CustomerArName = b.Customer?.ArName ?? "",
                CustomerEnName = b.Customer?.EnName ?? "",
                SupplierId = b.SupplierId,
                SupplierArName = b.Supplier?.ArName ?? "",
                SupplierEnName = b.Supplier?.EnName ?? "",
                Lines = b.BillDetails.Select(d => new BillMasterLineViewModel
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    LineTotal = d.LineTotal
                }).ToList()
            }).ToList();
        }
        public async Task<List<BillMasterViewModel>> GetBillReturnPurchasesAsync()
        {
            var bills = await _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.BillDetails)
                .ThenInclude(d => d.Product)
                .Include(d => d.Supplier)
                .Where(b => b.BillType == BillType.PurchaseReturn)
                .OrderByDescending(b => b.Amount)
                .ToListAsync();

            return bills.Select(b => new BillMasterViewModel
            {
                Id = b.Id,
                Code = b.Code,
                BillDate = b.BillDate,
                BillType = b.BillType,
                Amount = b.Amount,
                IsPosted = b.IsPosted,
                CustomerId = b.CustomerId,
                CustomerArName = b.Customer?.ArName ?? "",
                CustomerEnName = b.Customer?.EnName ?? "",
                SupplierId = b.SupplierId,
                SupplierArName = b.Supplier?.ArName ?? "",
                SupplierEnName = b.Supplier?.EnName ?? "",
                Lines = b.BillDetails.Select(d => new BillMasterLineViewModel
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    LineTotal = d.LineTotal
                }).ToList()
            }).ToList();
        }
        #endregion

        public async Task<DashboardSummaryViewModel> GetDashboardSummaryAsync()
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);

            var totalProducts = await _context.Products.AsNoTracking().CountAsync();
            var totalCustomers = await _context.Customers.AsNoTracking().CountAsync();
            var totalSuppliers = await _context.Suppliers.AsNoTracking().CountAsync();
            var totalEmployees = await _context.Employees.AsNoTracking().CountAsync();

            var todaySalesAmount = await _context.BillMasters.AsNoTracking()
                .Where(b => b.BillType == BillType.Sales && b.BillDate == today)
                .SumAsync(b => (decimal?)b.Amount) ?? 0;

            var monthSalesAmount = await _context.BillMasters.AsNoTracking()
                .Where(b => b.BillType == BillType.Sales && b.BillDate >= monthStart)
                .SumAsync(b => (decimal?)b.Amount) ?? 0;

            var monthPurchasesAmount = await _context.BillMasters.AsNoTracking()
                .Where(b => b.BillType == BillType.Purchase && b.BillDate >= monthStart)
                .SumAsync(b => (decimal?)b.Amount) ?? 0;

            var recentSales = await _context.BillMasters.AsNoTracking()
                .Include(b => b.Customer)
                .Where(b => b.BillType == BillType.Sales)
                .OrderByDescending(b => b.BillDate)
                .Take(5)
                .Select(b => new BillMasterViewModel
                {
                    Id = b.Id,
                    Code = b.Code,
                    BillDate = b.BillDate,
                    BillType = b.BillType,
                    Amount = b.Amount,
                    IsPosted = b.IsPosted,
                    CustomerArName = b.Customer != null ? b.Customer.ArName : "",
                    CustomerEnName = b.Customer != null ? b.Customer.EnName : ""
                })
                .ToListAsync();

            var recentPurchases = await _context.BillMasters.AsNoTracking()
                .Include(b => b.Supplier)
                .Where(b => b.BillType == BillType.Purchase)
                .OrderByDescending(b => b.BillDate)
                .Take(5)
                .Select(b => new BillMasterViewModel
                {
                    Id = b.Id,
                    Code = b.Code,
                    BillDate = b.BillDate,
                    BillType = b.BillType,
                    Amount = b.Amount,
                    IsPosted = b.IsPosted,
                    SupplierArName = b.Supplier != null ? b.Supplier.ArName : "",
                    SupplierEnName = b.Supplier != null ? b.Supplier.EnName : ""
                })
                .ToListAsync();

            var stockValuation = await GetStockValuationAsync();
            var totalStockValue = stockValuation.Sum(r => r.TotalValue);
            var topStockValueProducts = stockValuation
                .OrderByDescending(r => r.TotalValue)
                .Take(5)
                .ToList();

            return new DashboardSummaryViewModel
            {
                TotalProducts = totalProducts,
                TotalCustomers = totalCustomers,
                TotalSuppliers = totalSuppliers,
                TotalEmployees = totalEmployees,
                TodaySalesAmount = todaySalesAmount,
                MonthSalesAmount = monthSalesAmount,
                MonthPurchasesAmount = monthPurchasesAmount,
                TotalStockValue = totalStockValue,
                RecentSales = recentSales,
                RecentPurchases = recentPurchases,
                TopStockValueProducts = topStockValueProducts
            };
        }
    }
}
