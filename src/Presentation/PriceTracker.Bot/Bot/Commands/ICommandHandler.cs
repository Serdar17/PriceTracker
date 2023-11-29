using Telegram.Bot;
using Telegram.Bot.Types;

namespace PriceTracker.Bot.Bot.Commands;

public interface ICommandHandler
{
    Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default);
}