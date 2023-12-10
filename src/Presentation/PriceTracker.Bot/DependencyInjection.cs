using PriceTracker.BackgroundJob;
using PriceTracker.Bot.Configuration;
using PriceTracker.Commands;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.Product;
using PriceTracker.Services.User;

namespace PriceTracker.Bot;

public static class DependencyInjection
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddAppDbContext()
            .AddAppAutoMappers()
            .AddBackgroundJob()
            .AddAppUserServices()
            .AddAppProductServices()
            .AddAppCommands()
            ;
        
        return services;
    }
}