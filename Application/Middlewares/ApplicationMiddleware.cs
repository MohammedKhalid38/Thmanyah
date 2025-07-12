using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Hosting;

namespace Application.Middlewares;

public static class ApplicationMiddleware
{
    public static IApplicationBuilder UseApplicationBuilder(this IApplicationBuilder app, IWebHostEnvironment environment)
    {

        if (!environment.IsDevelopment())
        {
            // Configure the HTTP request pipeline.
            app.UseExceptionHandler("/Error/"); // Error handling middleware
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHsts();
            //app.UseHsts(options =>
            //{
            //    options.MaxAge(days: 365);
            //    options.IncludeSubdomains();
            //    options.Preload();
            //});
        }
        else
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        // Redirect http to https
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        //app.UseSecurityHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        //app.UseHangfireDashboard("/hangfire", new DashboardOptions
        //{
        //    DashboardTitle = Resources.BackgroundProcessingJobs,
        //    Authorization = new[]
        //    {
        //        new  HangfireAuthorizationFilter()
        //    }
        //});

        //Custom Middlewires:Start

        //Custom Middlewires:End
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.Use(async (context, next) =>
        {
            var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (httpMaxRequestBodySizeFeature is not null)
                httpMaxRequestBodySizeFeature.MaxRequestBodySize = long.MaxValue;

            await next(context);
        });

        return app;
    }
    public static IApplicationBuilder UseApplicationAdminBuilder(this IApplicationBuilder app, IWebHostEnvironment environment)
    {

        if (!environment.IsDevelopment())
        {
            // Configure the HTTP request pipeline.
            app.UseExceptionHandler("/Error/"); // Error handling middleware
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHsts();
        }
        else
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        // Redirect http to https
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        //app.UseSecurityHeaders();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        //app.UseHangfireDashboard("/hangfire", new DashboardOptions
        //{
        //    DashboardTitle = Resources.BackgroundProcessingJobs,
        //    Authorization = new[]
        //    {
        //        new  HangfireAuthorizationFilter()
        //    }
        //});

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.Use(async (context, next) =>
        {
            var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (httpMaxRequestBodySizeFeature is not null)
                httpMaxRequestBodySizeFeature.MaxRequestBodySize = long.MaxValue;

            await next(context);
        });

        return app;
    }
}
