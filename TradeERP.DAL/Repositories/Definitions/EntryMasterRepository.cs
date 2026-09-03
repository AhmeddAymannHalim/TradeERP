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
    public class EntryMasterRepository : GenericRepository<EntryMaster>, IEntryMasterRepository
    {
        private const int PageSize = 10;

        public EntryMasterRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ResultMessage> PostOpeningBalanceAsync(int ledgerAccountId, decimal amount, DebitCreditDirection direction, DateTime date)
        {
            var suspenseAccount = await _context.Set<LedgerAccount>()
                .FirstOrDefaultAsync(a => a.Code == SystemLedgerAccounts.OpeningBalanceEquity);
            if (suspenseAccount == null)
                return new ResultMessage { Success = false, Message = "OpeningBalanceEquityMissing" };

            var targetAccount = await _context.Set<LedgerAccount>().FindAsync(ledgerAccountId);
            if (targetAccount == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (await _context.Set<AccountingPeriod>().AnyAsync(p => p.IsClosed && date >= p.StartDate && date <= p.EndDate))
                return new ResultMessage { Success = false, Message = "PeriodIsClosed" };

            var debitAccountId = direction == DebitCreditDirection.Debit ? ledgerAccountId : suspenseAccount.Id;
            var creditAccountId = direction == DebitCreditDirection.Debit ? suspenseAccount.Id : ledgerAccountId;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entryCode = await GetNextEntryCodeAsync();

                var entryMaster = new EntryMaster
                {
                    Code = entryCode,
                    EntryDate = date,
                    Description = $"Opening balance for {targetAccount.EnName}",
                    EntryType = EntryType.OpeningBalance
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

        public async Task<string> GetNewCodeAsync()
        {
            var setting = await _context.Set<EntrySetting>().AsNoTracking().FirstOrDefaultAsync();
            return setting == null ? "1" : $"{setting.Prefix}{setting.NextNumber:D5}";
        }

        public async Task<EntryMaster?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<EntryMaster>()
                .Include(m => m.EntryDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        private static ResultMessage? ValidateLines(List<EntryDetails> lines)
        {
            if (lines.Count < 2)
                return new ResultMessage { Success = false, Message = "EntryNeedsAtLeastTwoLines" };

            foreach (var line in lines)
            {
                var hasDebit = line.DebitAmount > 0;
                var hasCredit = line.CreditAmount > 0;

                if (hasDebit == hasCredit)
                    return new ResultMessage { Success = false, Message = "EntryLineNeedsDebitOrCredit" };
            }

            var totalDebit = lines.Sum(l => l.DebitAmount);
            var totalCredit = lines.Sum(l => l.CreditAmount);

            if (totalDebit != totalCredit)
                return new ResultMessage { Success = false, Message = "EntryNotBalanced" };

            return null;
        }

        public async Task<ResultMessage> AddWithLinesAsync(EntryMaster entry, List<EntryDetails> lines)
        {
            var validationError = ValidateLines(lines);
            if (validationError != null)
                return validationError;

            if (await _context.Set<AccountingPeriod>().AnyAsync(p => p.IsClosed && entry.EntryDate >= p.StartDate && entry.EntryDate <= p.EndDate))
                return new ResultMessage { Success = false, Message = "PeriodIsClosed" };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                entry.EntryType = EntryType.Manual;
                entry.Code = await GetNextEntryCodeAsync();

                await _context.Set<EntryMaster>().AddAsync(entry);
                await _context.SaveChangesAsync();

                var lineNo = 1;
                foreach (var line in lines)
                {
                    line.EntryMasterId = entry.Id;
                    line.Code = $"{entry.Code}-{lineNo++}";
                }

                await _context.Set<EntryDetails>().AddRangeAsync(lines);
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

        public async Task<ResultMessage> UpdateWithLinesAsync(EntryMaster entry, List<EntryDetails> lines)
        {
            var existing = await _context.Set<EntryMaster>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == entry.Id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.EntryType != EntryType.Manual)
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            var validationError = ValidateLines(lines);
            if (validationError != null)
                return validationError;

            if (await _context.Set<AccountingPeriod>().AnyAsync(p => p.IsClosed && entry.EntryDate >= p.StartDate && entry.EntryDate <= p.EndDate))
                return new ResultMessage { Success = false, Message = "PeriodIsClosed" };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                entry.Code = existing.Code;
                entry.EntryType = EntryType.Manual;
                await base.UpdateAsync(entry);

                var oldLines = await _context.Set<EntryDetails>()
                    .Where(d => d.EntryMasterId == entry.Id)
                    .ToListAsync();
                _context.Set<EntryDetails>().RemoveRange(oldLines);
                await _context.SaveChangesAsync();

                var lineNo = 1;
                foreach (var line in lines)
                {
                    line.Id = 0;
                    line.EntryMasterId = entry.Id;
                    line.Code = $"{entry.Code}-{lineNo++}";
                }

                await _context.Set<EntryDetails>().AddRangeAsync(lines);
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

        public override async Task<ResultMessage> UpdateAsync(EntryMaster entity)
        {
            var existing = await _context.Set<EntryMaster>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == entity.Id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.EntryType != TradeERP.Shared.Enums.EntryType.Manual)
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            return await base.UpdateAsync(entity);
        }

        public override async Task<ResultMessage> DeleteAsync(int id)
        {
            var existing = await _context.Set<EntryMaster>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (existing == null)
                return new ResultMessage { Success = false, Message = "RecordNotFound" };

            if (existing.EntryType != TradeERP.Shared.Enums.EntryType.Manual)
                return new ResultMessage { Success = false, Message = "EntryIsSystemGenerated" };

            return await base.DeleteAsync(id);
        }

        public async Task<PaginatedResult<EntryMaster>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<EntryMaster>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(m => m.Code.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(m => m.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<EntryMaster>
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
