using System.Globalization;
using System.IO.Compression;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.DependencyInjection;
using TradeERP.DAL.Data;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.HelperServices.Services;

namespace TradeERP.PL.Extensions
{
   
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureAppSettings(this WebApplicationBuilder builder)
        {
            builder.AddAppDbContext();
            builder.AddAppMvc();
            builder.AddAppLocalization();
            builder.AddAppIdentity();
            builder.AddResponseCompressionConfigure();
            builder.AddAppSessionConfiguration();
            builder.Services.AddBllServices();
            builder.UpdateDefaultLimitationForFormInputs();
        }

        private static void AddAppDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.CommandTimeout(60)));
        }

        private static void AddAppIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        private static void AddResponseCompressionConfigure(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });

            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
                options.Level = CompressionLevel.Optimal);

            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = CompressionLevel.SmallestSize);
        }

        private static void AddAppSessionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });
        }

        private static void UpdateDefaultLimitationForFormInputs(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 10000;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
                options.Limits.MaxRequestBodySize = long.MaxValue);
        }

        private static void AddAppMvc(this WebApplicationBuilder builder)
        {
            var mvcBuilder = builder.Services.AddControllersWithViews();

            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
        }

        private static void AddAppLocalization(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var arCulture = new CultureInfo("ar")
                {
                    NumberFormat =
                    {
                        NativeDigits = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                        NumberDecimalSeparator = ".",
                        CurrencyDecimalSeparator = ".",
                        PercentDecimalSeparator = "."
                    },
                    DateTimeFormat = { Calendar = new GregorianCalendar() }
                };
                var enCulture = new CultureInfo("en");

                var supportedCultures = new[] { enCulture, arCulture };

                options.DefaultRequestCulture = new RequestCulture(enCulture, enCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
    }
}
