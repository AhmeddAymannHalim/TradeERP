using Microsoft.Extensions.Caching.Memory;

namespace TradeERP.Shared.HelperServices.Interfaces
{
    public interface IMemoryCacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> acquire, MemoryCacheEntryOptions? options = null);
        void Remove(string cacheKey);
    }
}
