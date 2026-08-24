using Microsoft.AspNetCore.Http;

namespace TradeERP.Shared.Extensions
{
    /// <summary>
    /// Static cookie access helper for code that doesn't have direct access to HttpContext
    /// (e.g. static utility classes). Must be configured once in Program.cs via Configure().
    /// </summary>
    public static class CookieHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static HttpContext HttpContext => _httpContextAccessor?.HttpContext
            ?? throw new InvalidOperationException("HttpContext is not available. Call CookieHelper.Configure() in Program.cs.");

        private static CookieOptions DefaultOptions => new()
        {
            HttpOnly = true,
            Secure = false, // Set true in production (HTTPS only)
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            Path = "/"
        };

        public static void SetCookie(string key, string value, CookieOptions? options = null)
        {
            options ??= DefaultOptions;
            HttpContext.Response.Cookies.Append(key, value, options);
        }

        public static string? GetCookie(string key) => HttpContext.Request.Cookies[key];

        public static void RemoveCookie(string key) => HttpContext.Response.Cookies.Delete(key);
    }
}
