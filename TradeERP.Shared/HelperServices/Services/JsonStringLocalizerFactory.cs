using Microsoft.Extensions.Localization;
using TradeERP.Shared.HelperServices.Interfaces;

namespace TradeERP.Shared.HelperServices.Services
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IMemoryCacheService _cacheService;

        public JsonStringLocalizerFactory(IMemoryCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_cacheService);

        public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_cacheService);
    }
}
