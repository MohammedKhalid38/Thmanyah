using Application.Commons;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Persistence;


namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
    {

        services.AddSingleton<IRecaptcha, Recaptcha>();
        services.AddPersistenceServices(builder)
                .AddInfrastructureServices(builder);

        return services;
    }
}
