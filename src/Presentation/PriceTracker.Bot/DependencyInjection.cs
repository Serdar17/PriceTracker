using PriceTracker.BackgroundJob;
using PriceTracker.Bot.Bot.Commands;
using PriceTracker.Bot.Bot.Factory;
using PriceTracker.Bot.Configuration;
using PriceTracker.Infrastructure.Context;

namespace PriceTracker.Bot;

public static class DependencyInjection
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddTransient<ICommandHandler, StartCommandHandler>()
            .AddTransient<ICommandHandler, AddCommandHandler>()
            .AddTransient<ICommandHandlerFactory, CommandHandlerFactory>();
       
        services
            .AddAppDbContext()
            .AddAppAutoMappers()
            .AddBackgroundJob()
            ;
        
        return services;
    }
}