namespace PriceTracker.Bot.Options;

public class TelegramBotSettings
{
    public const string SectionName = "TelegramBotSettings";
    public string Token { get; set; } = string.Empty;
}