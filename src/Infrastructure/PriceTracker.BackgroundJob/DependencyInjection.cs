using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace PriceTracker.BackgroundJob;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundJob(this IServiceCollection services)
    {
        services.AddQuartz(opt =>
        {
            opt.UseMicrosoftDependencyInjectionJobFactory();
        });
        
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        
        services.ConfigureOptions<ParsingBackgroundJobSetup>();
        return services;
    }
}