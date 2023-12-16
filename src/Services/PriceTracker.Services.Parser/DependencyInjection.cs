using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace PriceTracker.Services.Parser;

public static class DependencyInjection
{
    public static IServiceCollection AddAppParsers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromCallingAssembly()
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Append)
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        return services;
    }
}