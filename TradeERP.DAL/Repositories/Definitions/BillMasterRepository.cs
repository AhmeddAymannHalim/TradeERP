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

        public async Task<ResultMessage> PostBillAsync(int id)
        {
            var bill = await GetByIdWithDetailsAsync(id);
            if (bill == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (bill.IsPosted)
                return new ResultMessage { Success = false, Message = "BillAlreadyPosted" };

            if (!bill.BillDetails.Any())
                return new ResultMessage { Success = false, Message = "BillHasNoDetails" };

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
                bill.Amount = amount;
                bill.IsPosted = true;

                var entryCode = await GetNextEntryCodeAsync();

                var entryMaster = new EntryMaster
                {
                    Code = entryCode,
                    EntryDate = bill.BillDate,
                    Description = $"Auto-posted from Bill {bill.Code}",
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

                await transaction.CommitAsync();
                return new ResultMessage { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultMessage { Success = false, Message = ex.Message };
            }
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
