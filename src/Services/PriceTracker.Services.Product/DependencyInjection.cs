using Microsoft.Extensions.DependencyInjection;

namespace PriceTracker.Services.Product;

public static class DependencyInjection
{
    public static IServiceCollection AddAppProductServices(this IServiceCollection services)
    {
        services.AddSingleton<IProductService, ProductService>();
        
        return services;
    }
}