using Microsoft.Extensions.Options;
using Quartz;

namespace PriceTracker.BackgroundJob;

public class ParsingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    private static readonly TimeSpan TimeInMinutes = TimeSpan.FromMinutes(5);

    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(ParsingBackgroundJob));
        options
            .AddJob<ParsingBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => 
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule => 
                        schedule
                            .WithIntervalInSeconds((int)TimeInMinutes.TotalSeconds).RepeatForever()));
    }
}