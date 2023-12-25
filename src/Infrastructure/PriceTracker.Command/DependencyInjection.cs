using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace PriceTracker.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddAppCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromCallingAssembly()
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Append)
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        return services;
    }
    
}