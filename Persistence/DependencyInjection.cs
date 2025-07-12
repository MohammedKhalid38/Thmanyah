using Domain.IdentityModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 3;
        }).AddEntityFrameworkStores<DataContext>()
          .AddDefaultTokenProviders();


        var dbContext = services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<DataContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();

        return services;
    }
}
