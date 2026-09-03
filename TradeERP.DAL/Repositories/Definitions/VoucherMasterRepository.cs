using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.Definitions;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Commons;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.DAL.Repositories.Definitions
{
    public class VoucherMasterRepository : GenericRepository<VoucherMaster>, IVoucherMasterRepository
    {
        private const int PageSize = 10;

        public VoucherMasterRepository(ApplicationDbContext context) : base(context) { }

        public async Task<int> GetNewCodeAsync()
        {
            var codes = await _context.Set<VoucherMaster>().Select(v => v.Code).ToListAsync();

            return codes
                .Where(c => int.TryParse(c, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        public async Task<PaginatedResult<VoucherMaster>> GetPagedAsync(int pageNo, string? searchString)
        {
            var query = _context.Set<VoucherMaster>().AsNoTracking()
                .Include(v => v.Customer)
                .Include(v => v.Supplier)
                .Include(v => v.TreasuryLedgerAccount)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();
                query = query.Where(v => v.Code.ToLower().Contains(searchLower));
            }

            var totalRecords = await query.CountAsync();
            var data = await query
                .OrderBy(v => v.Id)
                .Skip((pageNo - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PaginatedResult<VoucherMaster>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling((double)totalRecords / PageSize),
                SearchString = searchString
            };
        }

        public async Task<ResultMessage> AddAndPostAsync(VoucherMaster voucher)
        {
            int partyLedgerAccountId;

            if (voucher.VoucherType == VoucherType.Receipt)
            {
                if (!voucher.CustomerId.HasValue)
                    return new ResultMessage { Success = false, Message = "CustomerRequired" };

                var customer = await _context.Set<Customer>().FindAsync(voucher.CustomerId.Value);
                if (customer?.LedgerAccountId == null)
                    return new ResultMessage { Success = false, Message = "PartyLedgerAccountMissing" };

                partyLedgerAccountId = customer.LedgerAccountId.Value;
            }
            else
            {
                if (!voucher.SupplierId.HasValue)
                    return new ResultMessage { Success = false, Message = "SupplierRequired" };

                var supplier = await _context.Set<Supplier>().FindAsync(voucher.SupplierId.Value);
                if (supplier?.LedgerAccountId == null)
                    return new ResultMessage { Success = false, Message = "PartyLedgerAccountMissing" };

                partyLedgerAccountId = supplier.LedgerAccountId.Value;
            }

            var treasuryAccount = await _context.Set<LedgerAccount>().FindAsync(voucher.TreasuryLedgerAccountId);
            if (treasuryAccount == null)
                return new ResultMessage { Success = false, Message = "TreasuryLedgerAccountMissing" };

            var debitAccountId = voucher.VoucherType == VoucherType.Receipt ? voucher.TreasuryLedgerAccountId : partyLedgerAccountId;
            var creditAccountId = voucher.VoucherType == VoucherType.Receipt ? partyLedgerAccountId : voucher.TreasuryLedgerAccountId;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                voucher.IsPosted = true;
                await _context.Set<VoucherMaster>().AddAsync(voucher);
                await _context.SaveChangesAsync();

                var entryMaster = new EntryMaster
                {
                    Code = $"JE-{voucher.Code}",
                    EntryDate = voucher.VoucherDate,
                    Description = $"Auto-posted from Voucher {voucher.Code}",
                    SourceVoucherMasterId = voucher.Id
                };
                await _context.Set<EntryMaster>().AddAsync(entryMaster);
                await _context.SaveChangesAsync();

                await _context.Set<EntryDetails>().AddRangeAsync(
                    new EntryDetails
                    {
                        Code = $"JE-{voucher.Code}-D",
                        EntryMasterId = entryMaster.Id,
                        LedgerAccountId = debitAccountId,
                        DebitAmount = voucher.Amount,
                        CreditAmount = 0
                    },
                    new EntryDetails
                    {
                        Code = $"JE-{voucher.Code}-C",
                        EntryMasterId = entryMaster.Id,
                        LedgerAccountId = creditAccountId,
                        DebitAmount = 0,
                        CreditAmount = voucher.Amount
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
    }
}
