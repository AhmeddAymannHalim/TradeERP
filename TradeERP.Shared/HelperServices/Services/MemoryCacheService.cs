using Microsoft.Extensions.Caching.Memory;
using TradeERP.Shared.HelperServices.Interfaces;

namespace TradeERP.Shared.HelperServices.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrSet<T>(string cacheKey, Func<T> acquire, MemoryCacheEntryOptions? options = null)
        {
            if (_cache.TryGetValue(cacheKey, out T? cached) && cached is not null)
                return cached;

            var value = acquire();
            _cache.Set(cacheKey, value, options ?? new MemoryCacheEntryOptions());
            return value;
        }

        public void Remove(string cacheKey) => _cache.Remove(cacheKey);
    }
}
