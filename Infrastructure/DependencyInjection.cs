using AutoDependencyRegistration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AutoRegisterDependencies();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //services.Configure<GoogleRecaptcha>(builder.Configuration.GetSection("GoogleRecaptcha"));

        services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
        services.AddMemoryCache();
        services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddHttpClient();

        // Serilog configuration
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "SbcWebite-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        services.AddSingleton(Log.Logger);

        services.AddSession(options =>
        {
            options.Cookie.Name = ".AspNetCore.Session";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.IsEssential = true;
        });

        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        //services.AddAuthentication();
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.Always;
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
        });

        services.AddAuthorization();
        services.AddControllersWithViews();
        services.AddMvcCore(options =>
        {
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "This field is required.");
            options.RequireHttpsPermanent = true;
            options.RespectBrowserAcceptHeader = true;
            options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options));
        })
        .AddRazorPages()
        .AddRazorRuntimeCompilation()
        .AddFormatterMappings()
        .AddCookieTempDataProvider(options =>
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.HttpOnly = true;
            options.Cookie.Expiration = TimeSpan.FromMinutes(30);
        });

        //services.AddHangfire(config => config.UseMemoryStorage());
        //services.AddHangfireServer();

        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = long.MaxValue;
            options.MultipartBoundaryLengthLimit = int.MaxValue;
            options.MultipartHeadersCountLimit = int.MaxValue;
            options.MultipartHeadersLengthLimit = int.MaxValue;
            options.ValueCountLimit = int.MaxValue;
        });

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = long.MaxValue;
            options.Limits.MaxRequestBufferSize = long.MaxValue;
            options.Limits.MaxRequestLineSize = int.MaxValue;
            options.Limits.MaxRequestHeadersTotalSize = int.MaxValue;
            options.Limits.MaxResponseBufferSize = int.MaxValue;
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
            options.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromSeconds(30);
            options.Limits.Http2.KeepAlivePingTimeout = TimeSpan.FromMinutes(1);
        })
        .UseIISIntegration();

        return services;
    }

    public static IServiceCollection AddAPIInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AutoRegisterDependencies();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
        services.AddMemoryCache();
        services.AddRouting(options => options.LowercaseUrls = true);

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "SbcWebite-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        services.AddSingleton(Log.Logger);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxRequestBodySize = long.MaxValue;
            options.Limits.MaxRequestBufferSize = long.MaxValue;
            options.Limits.MaxRequestLineSize = int.MaxValue;
            options.Limits.MaxRequestHeadersTotalSize = int.MaxValue;
            options.Limits.MaxResponseBufferSize = int.MaxValue;
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
            options.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromSeconds(30);
            options.Limits.Http2.KeepAlivePingTimeout = TimeSpan.FromMinutes(1);
        })
        .UseIISIntegration();

        return services;
    }
}

