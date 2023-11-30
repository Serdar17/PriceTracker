using PriceTracker.Bot;
using PriceTracker.Bot.Bot;
using PriceTracker.Bot.Configuration;
using PriceTracker.Bot.Options;
using PriceTracker.Infrastructure.Context.Setup;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.AddAppLogger();

builder.Services.Configure<TelegramBotSettings>(
    builder.Configuration.GetSection(TelegramBotSettings.SectionName));

services.AddSingleton<PriceTrackerBot>();
services.RegisterAppServices();

var app = builder.Build();

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetService<PriceTrackerBot>();

DbInitializer.Execute(app.Services);
DbSeeder.Execute(app.Services, true, true);

app.Run();