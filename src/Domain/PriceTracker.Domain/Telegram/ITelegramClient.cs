namespace PriceTracker.Domain.Telegram;

public interface ITelegramClient
{
    public Task SendPriceChangingNotification(long chatId, string message,
        CancellationToken cancellationToken = default);
}