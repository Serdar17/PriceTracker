using PriceTracker.Infrastructure.Context;

namespace PriceTracker.Bot;

public static class DependencyInjection
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddAppDbContext()
            ;
        
        return services;
    }
}