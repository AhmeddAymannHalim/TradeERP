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

                var entryMaster = new EntryMaster
                {
                    Code = $"JE-{bill.Code}",
                    EntryDate = bill.BillDate,
                    Description = $"Auto-posted from Bill {bill.Code}",
                    SourceBillMasterId = bill.Id
                };
                await _context.Set<EntryMaster>().AddAsync(entryMaster);
                await _context.SaveChangesAsync();

                await _context.Set<EntryDetails>().AddRangeAsync(
                    new EntryDetails
                    {
                        Code = $"JE-{bill.Code}-D",
                        EntryMasterId = entryMaster.Id,
                        LedgerAccountId = debitAccountId,
                        DebitAmount = amount,
                        CreditAmount = 0
                    },
                    new EntryDetails
                    {
                        Code = $"JE-{bill.Code}-C",
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
