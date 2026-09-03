using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;

namespace TradeERP.DAL.Repositories.Commons
{
    /// <summary>
    /// Serializes concurrent access to a business-key resource (e.g. "a numbering sequence
    /// for product X's stock") using SQL Server's sp_getapplock, scoped to the CALLER'S
    /// ambient transaction so the lock releases automatically on commit or rollback.
    /// Must be called from inside an already-open transaction.
    /// </summary>
    public static class SqlLockHelper
    {
        public static async Task AcquireTransactionLockAsync(ApplicationDbContext context, string resource)
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_getapplock @Resource = {resource}, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 30000");
        }
    }
}
