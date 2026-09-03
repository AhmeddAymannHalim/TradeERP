using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.Constants;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class BillMasterRepository : GenericRepository<BillMaster>, IBillMasterRepository
    {
        private const int PageSize = 10;

        public BillMasterRepository(ApplicationDbContext context) : base(context) { }

        public async Task<BillMaster?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<BillMaster>()
                .Include(b => b.Customer)
                .Include(b => b.Supplier)
                .Include(b => b.BillDetails)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<EntryMaster?> GetJournalEntryAsync(int billMasterId)
        {
            return await _context.Set<EntryMaster>()
                .Include(e => e.EntryDetails).ThenInclude(d => d.LedgerAccount)
                .FirstOrDefaultAsync(e => e.SourceBillMasterId == billMasterId);
        }

        public async Task<ResultMessage> PostBillAsync(int id)
        {
            var bill = await GetByIdWithDetailsAsync(id);
            if (bill == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (bill.IsPosted)
                return new ResultMessage { Success = false, Message = "BillAlreadyPosted" };

            if (!bill.BillDetails.Any())
                return new ResultMessage { Success = false, Message = "BillHasNoDetails" };

            if (await _context.Set<AccountingPeriod>().AnyAsync(p => p.IsClosed && bill.BillDate >= p.StartDate && bill.BillDate <= p.EndDate))
                return new ResultMessage { Success = false, Message = "PeriodIsClosed" };

            var isPurchaseSide = bill.BillType is BillType.Purchase or BillType.PurchaseReturn;

            var partyLedgerAccountId = isPurchaseSide
                ? bill.Supplier?.LedgerAccountId
                : bill.Customer?.LedgerAccountId;

            if (partyLedgerAccountId == null)
                return new ResultMessage { Success = false, Message = "PartyLedgerAccountMissing" };

            var systemAccountCode = isPurchaseSide
                ? SystemLedgerAccounts.PurchaseExpense
                : SystemLedgerAccounts.SalesRevenue;

            var systemAccount = await _context.Set<LedgerAccount>().FirstOrDefaultAsync(a => a.Code == systemAccountCode);
            if (systemAccount == null)
                return new ResultMessage { Success = false, Message = "SystemLedgerAccountMissing" };

            var amount = bill.BillDetails.Sum(d => d.LineTotal);

            int debitAccountId;
            int creditAccountId;
            var stockMovement = bill.BillType is BillType.Sales or BillType.PurchaseReturn
                ? StockMovementType.Out
                : StockMovementType.In;

            switch (bill.BillType)
            {
                case BillType.Sales:
                    debitAccountId = partyLedgerAccountId.Value;
                    creditAccountId = systemAccount.Id;
                    break;
                case BillType.Purchase:
                    debitAccountId = systemAccount.Id;
                    creditAccountId = partyLedgerAccountId.Value;
                    break;
                case BillType.SalesReturn:
                    debitAccountId = systemAccount.Id;
                    creditAccountId = partyLedgerAccountId.Value;
                    break;
                case BillType.PurchaseReturn:
                    debitAccountId = partyLedgerAccountId.Value;
                    creditAccountId = systemAccount.Id;
                    break;
                default:
                    return new ResultMessage { Success = false, Message = "UnsupportedBillType" };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Serialize any other post touching the same product(s) - held for the
                // lifetime of this transaction - so the stock check below can't race with
                // a concurrent post reading the same pre-write snapshot (oversell guard).
                foreach (var productId in bill.BillDetails.Select(d => d.ProductId).Distinct())
                {
                    await SqlLockHelper.AcquireTransactionLockAsync(_context, $"Stock_Product_{productId}");
                }

                // Compute stock movements (and reject if an Out would take a product negative)
                // now that concurrent posts for the same product(s) are locked out.
                var stockMoves = new List<(int ProductId, decimal Quantity, decimal UnitCost)>();
                var pendingBalance = new Dictionary<int, decimal>();

                foreach (var line in bill.BillDetails)
                {
                    decimal unitCost;
                    if (bill.BillType is BillType.Purchase or BillType.PurchaseReturn)
                    {
                        unitCost = line.UnitPrice;
                    }
                    else
                    {
                        var (_, avgCost) = await GetStockPositionAsync(line.ProductId);
                        unitCost = avgCost;
                    }

                    if (stockMovement == StockMovementType.Out)
                    {
                        if (!pendingBalance.TryGetValue(line.ProductId, out var available))
                        {
                            (available, _) = await GetStockPositionAsync(line.ProductId);
                        }

                        if (available < line.Quantity)
                        {
                            await transaction.RollbackAsync();
                            return new ResultMessage { Success = false, Message = "InsufficientStock" };
                        }

                        pendingBalance[line.ProductId] = available - line.Quantity;
                    }

                    stockMoves.Add((line.ProductId, line.Quantity, unitCost));
                }

                bill.Amount = amount;
                bill.IsPosted = true;

                var entryCode = await GetNextEntryCodeAsync();

                var entryMaster = new EntryMaster
                {
                    Code = entryCode,
                    EntryDate = bill.BillDate,
                    Description = $"Auto-posted from Bill {bill.Code}",
                    EntryType = EntryType.BillPosting,
                    SourceBillMasterId = bill.Id
                };
                await _context.Set<EntryMaster>().AddAsync(entryMaster);
                await _context.SaveChangesAsync();

                await _context.Set<EntryDetails>().AddRangeAsync(
                    new EntryDetails
                    {
                        Code = $"{entryCode}-D",
                        EntryMasterId = entryMaster.Id,
                        LedgerAccountId = debitAccountId,
                        DebitAmount = amount,
                        CreditAmount = 0
                    },
                    new EntryDetails
                    {
                        Code = $"{entryCode}-C",
                        EntryMasterId = entryMaster.Id,
                        LedgerAccountId = creditAccountId,
                        DebitAmount = 0,
                        CreditAmount = amount
                    });
                await _context.SaveChangesAsync();

                var stockLedgerRows = stockMoves.Select(m => new StockLedger
                {
                    ProductId = m.ProductId,
                    MovementDate = bill.BillDate,
                    MovementType = stockMovement,
                    Quantity = m.Quantity,
                    UnitCost = m.UnitCost,
                    SourceType = StockSourceType.Bill,
                    SourceId = bill.Id
                });
                await _context.Set<StockLedger>().AddRangeAsync(stockLedgerRows);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// Current on-hand quantity and weighted-average cost for a product, derived from
        /// StockLedger (remaining value / remaining qty = SUM(In) - SUM(Out at the cost each
        /// Out was recorded at), never a stored running balance.
        /// </summary>
        private async Task<(decimal Quantity, decimal AverageCost)> GetStockPositionAsync(int productId)
        {
            var moves = await _context.Set<StockLedger>()
                .AsNoTracking()
                .Where(s => s.ProductId == productId)
                .Select(s => new { s.MovementType, s.Quantity, s.UnitCost })
                .ToListAsync();

            var inQty = moves.Where(m => m.MovementType == StockMovementType.In).Sum(m => m.Quantity);
            var inValue = moves.Where(m => m.MovementType == StockMovementType.In).Sum(m => m.Quantity * m.UnitCost);
            var outQty = moves.Where(m => m.MovementType == StockMovementType.Out).Sum(m => m.Quantity);
            var outValue = moves.Where(m => m.MovementType == StockMovementType.Out).Sum(m => m.Quantity * m.UnitCost);

            var remainingQty = inQty - outQty;
            var remainingValue = inValue - outValue;
            var averageCost = remainingQty > 0 ? remainingValue / remainingQty : 0;

            return (remainingQty, averageCost);
        }

        public async Task<ResultMessage> AddWithDetailsAsync(BillMaster bill, List<BillDetails> lines)
        {
            if (lines.Count == 0)
                return new ResultMessage { Success = false, Message = "BillHasNoDetails" };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bill.Code = await GetNextBillCodeAsync();

                await _context.Set<BillMaster>().AddAsync(bill);
                await _context.SaveChangesAsync();

                var lineNo = 1;
                foreach (var line in lines)
                {
                    line.BillMasterId = bill.Id;
                    line.Code = $"{bill.Code}-{lineNo++}";
                    line.LineTotal = line.Quantity * line.UnitPrice;
                }

                await _context.Set<BillDetails>().AddRangeAsync(lines);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultMessage> AddWithDetailsAndPostAsync(BillMaster bill, List<BillDetails> lines)
        {
            var addResult = await AddWithDetailsAsync(bill, lines);
            if (!addResult.Success)
                return addResult;

            var postResult = await PostBillAsync(bill.Id);
            if (!postResult.Success)
                return new ResultMessage { Success = true, Data = postResult.Message };

            return new ResultMessage { Success = true };
        }

        public async Task<ResultMessage> UpdateWithDetailsAsync(BillMaster bill, List<BillDetails> lines)
        {
            var existing = await _context.Set<BillMaster>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == bill.Id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.IsPosted)
                return new ResultMessage { Success = false, Message = "BillAlreadyPosted" };

            if (lines.Count == 0)
                return new ResultMessage { Success = false, Message = "BillHasNoDetails" };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await base.UpdateAsync(bill);

                var oldLines = await _context.Set<BillDetails>()
                    .Where(d => d.BillMasterId == bill.Id)
                    .ToListAsync();
                _context.Set<BillDetails>().RemoveRange(oldLines);
                await _context.SaveChangesAsync();

                var lineNo = 1;
                foreach (var line in lines)
                {
                    line.Id = 0;
                    line.BillMasterId = bill.Id;
                    line.Code = $"{bill.Code}-{lineNo++}";
                    line.LineTotal = line.Quantity * line.UnitPrice;
                }

                await _context.Set<BillDetails>().AddRangeAsync(lines);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultMessage { Success = false, Message = ex.Message };
            }
        }

        public override async Task<ResultMessage> UpdateAsync(BillMaster entity)
        {
            var existing = await _context.Set<BillMaster>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == entity.Id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.IsPosted)
                return new ResultMessage { Success = false, Message = "BillAlreadyPosted" };

            return await base.UpdateAsync(entity);
        }

        public override async Task<ResultMessage> DeleteAsync(int id)
        {
            var existing = await _context.Set<BillMaster>().AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.IsPosted)
                return new ResultMessage { Success = false, Message = "BillAlreadyPosted" };

            return await base.DeleteAsync(id);
        }

        public async Task<string> GetNewCodeAsync()
        {
            var setting = await _context.Set<BillSetting>().AsNoTracking().FirstOrDefaultAsync();
            return setting == null ? "1" : $"{setting.Prefix}{setting.NextNumber:D5}";
        }

        private async Task<string> GetNextBillCodeAsync()
        {
            await SqlLockHelper.AcquireTransactionLockAsync(_context, "Sequence_BillSetting");

            var setting = await _context.Set<BillSetting>().FirstOrDefaultAsync();
            if (setting == null)
                return DateTime.UtcNow.Ticks.ToString();

            var code = $"{setting.Prefix}{setting.NextNumber:D5}";
            setting.NextNumber++;
            await _context.SaveChangesAsync();
            return code;
        }

        private async Task<string> GetNextEntryCodeAsync()
        {
            await SqlLockHelper.AcquireTransactionLockAsync(_context, "Sequence_EntrySetting");

            var setting = await _context.Set<EntrySetting>().FirstOrDefaultAsync();
            if (setting == null)
                return $"JE-{DateTime.UtcNow.Ticks}";

            var code = $"{setting.Prefix}{setting.NextNumber:D5}";
            setting.NextNumber++;
            await _context.SaveChangesAsync();
            return code;
        }

        public async Task<PaginatedResult<BillMaster>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<BillMaster>().AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(b => b.Code.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(b => b.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<BillMaster>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }
    }
}
