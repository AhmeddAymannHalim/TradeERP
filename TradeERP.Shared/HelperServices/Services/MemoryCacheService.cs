using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.Options;

namespace TradeERP.Shared.HelperServices.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly CacheOptions _cacheOptions;

        public MemoryCacheService(IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
        {
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
        }

        public T GetOrSet<T>(string cacheKey, Func<T> acquire, MemoryCacheEntryOptions? options = null)
        {
            if (_cache.TryGetValue(cacheKey, out T? cached) && cached is not null)
                return cached;

            var value = acquire();
            _cache.Set(cacheKey, value, options ?? BuildDefaultOptions());
            return value;
        }

        public void Remove(string cacheKey) => _cache.Remove(cacheKey);

        // BEFORE: `options ?? new MemoryCacheEntryOptions()` set no expiration at all,
        // so every localization key ever read stayed in RAM for the app's entire lifetime
        // (a slow, unbounded memory leak). This gives every cache entry a real lifespan.
        private MemoryCacheEntryOptions BuildDefaultOptions() => new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(_cacheOptions.SlidingExpirationMinutes),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOptions.AbsoluteExpirationRelativeToNowMinutes)
        };
    }
}
