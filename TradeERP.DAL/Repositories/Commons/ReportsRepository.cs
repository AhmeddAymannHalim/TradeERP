using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.Models;
using TradeERP.Shared.Enums;
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
            // Aggregate server-side (one row per product comes back, not one per movement) -
            // same principle as GetTrialBalanceAsync above, instead of pulling the whole
            // StockLedger history into memory to group in C#.
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
    }
}
