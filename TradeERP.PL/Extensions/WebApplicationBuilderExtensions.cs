using AspNet.Security.OAuth.Apple;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using TradeERP.BLL;
using TradeERP.DAL.Data;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.HelperServices.Services;
using TradeERP.Shared.Options;

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
            builder.AddAppJwtAuth();
            builder.AddAppExternalAuth();
            builder.AddAppEmail();
            builder.AddResponseCompressionConfigure();
            builder.AddAppSessionConfiguration();
            builder.Services.AddBllServices();
            builder.UpdateDefaultLimitationForFormInputs();
        }

        private static void AddAppDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IToastrService, ToastrService>();

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

        private static void AddAppJwtAuth(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.SectionName));
            builder.Services.AddScoped<ITokenService, TokenService>();

            var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("Jwt configuration section is missing.");

            if (string.IsNullOrWhiteSpace(jwtOptions.Key))
                throw new InvalidOperationException("Jwt:Key must be configured (e.g. via appsettings.Development.json or user secrets).");

            builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });
        }

        private static void AddAppExternalAuth(this WebApplicationBuilder builder)
        {
            var authBuilder = builder.Services.AddAuthentication();
            var config = builder.Configuration;

            var googleClientId = config["Authentication:Google:ClientId"];
            var googleClientSecret = config["Authentication:Google:ClientSecret"];
            if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
            {
                authBuilder.AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                });
            }

            var facebookAppId = config["Authentication:Facebook:AppId"];
            var facebookAppSecret = config["Authentication:Facebook:AppSecret"];
            if (!string.IsNullOrWhiteSpace(facebookAppId) && !string.IsNullOrWhiteSpace(facebookAppSecret))
            {
                authBuilder.AddFacebook(options =>
                {
                    options.AppId = facebookAppId;
                    options.AppSecret = facebookAppSecret;
                });
            }

            var appleClientId = config["Authentication:Apple:ClientId"];
            var appleKeyId = config["Authentication:Apple:KeyId"];
            var appleTeamId = config["Authentication:Apple:TeamId"];
            var applePrivateKey = config["Authentication:Apple:PrivateKey"];
            if (!string.IsNullOrWhiteSpace(appleClientId) && !string.IsNullOrWhiteSpace(appleKeyId)
                && !string.IsNullOrWhiteSpace(appleTeamId) && !string.IsNullOrWhiteSpace(applePrivateKey))
            {
                authBuilder.AddApple(options =>
                {
                    options.ClientId = appleClientId;
                    options.KeyId = appleKeyId;
                    options.TeamId = appleTeamId;
                    options.PrivateKey = (keyId, cancellationToken) => Task.FromResult(applePrivateKey.AsMemory());
                });
            }
        }

        private static void AddAppEmail(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<EmailOptions>(
                builder.Configuration.GetSection(EmailOptions.SectionName));
            builder.Services.AddScoped<IEmailService, EmailService>();
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
            var mvcBuilder = builder.Services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());

                var requireAuthenticatedUser = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(requireAuthenticatedUser));
            });

            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }

        private static void AddAppLocalization(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<CacheOptions>(
                builder.Configuration.GetSection(CacheOptions.SectionName));
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
