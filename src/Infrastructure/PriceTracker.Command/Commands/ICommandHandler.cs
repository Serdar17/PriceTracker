using Telegram.Bot;
using Telegram.Bot.Types;

namespace PriceTracker.Commands.Commands;

public interface ICommandHandler
{
    Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default);
    Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
}