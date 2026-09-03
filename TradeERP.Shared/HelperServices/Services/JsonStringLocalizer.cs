using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using TradeERP.Shared.HelperServices.Interfaces;

namespace TradeERP.Shared.HelperServices.Services
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly string _cultureName;
        private readonly IMemoryCacheService _memoryCacheService;

        private string CacheKey => $"Locale.{_cultureName}";
        private string RelativeFilePath => Path.Combine("Resources", $"{_cultureName}.json");

        public JsonStringLocalizer(IMemoryCacheService memoryCacheService, string? cultureName = null)
        {
            _memoryCacheService = memoryCacheService;
            _cultureName = cultureName ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        }

        private static readonly JsonSerializerOptions FileOptions = new()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private static Dictionary<string, string> LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new Dictionary<string, string>();

            using var stream = File.OpenRead(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(stream, FileOptions) ?? new Dictionary<string, string>();
        }

        private Dictionary<string, string> GetAllValues()
        {
            return _memoryCacheService.GetOrSet(CacheKey, () =>
            {
                var fullPath = Path.GetFullPath(RelativeFilePath);
                return LoadFile(fullPath);
            }, new MemoryCacheEntryOptions());
        }

        private string GetString(string resourceKey)
        {
            var values = GetAllValues();
            return values.TryGetValue(resourceKey, out var value) ? value : string.Empty;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                var resourceNotFound = string.IsNullOrWhiteSpace(value);
                return new LocalizedString(name, resourceNotFound ? name : value, resourceNotFound);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var localizedString = this[name];
                return !localizedString.ResourceNotFound
                    ? new LocalizedString(name, string.Format(localizedString.Value, arguments), false)
                    : localizedString;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            foreach (var kvp in GetAllValues())
                yield return new LocalizedString(kvp.Key, kvp.Value, false);
        }
    }
}
