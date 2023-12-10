using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Commands.Commands;
using Scrutor;

namespace PriceTracker.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddAppCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<ICommandHandler>()
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithTransientLifetime());
        
        return services;
    }
    
}