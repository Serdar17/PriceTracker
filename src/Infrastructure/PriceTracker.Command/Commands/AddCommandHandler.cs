using PriceTracker.Common.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PriceTracker.Commands.Commands;

public class AddCommandHandler : ICommandHandler
{
    public AddCommandHandler()
    {
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken = default)
    {
        if (update.Message is not { } message)
            return;

        var keyboards = GetInlineKeyboardMarkup();

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите доступный сайт для парсинга цены",
            replyMarkup: keyboards,
            cancellationToken: cancellationToken);
    }

    public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        if (callbackQuery.Message is not { } message)
            return;
        
        if (callbackQuery.Data is null)
            return;

        var marketplaceName = callbackQuery.Data.Split().Skip(1);
        await botClient.SendTextMessageAsync(
            message.Chat.Id,
            $"Пожалуйста, вставьте ссылку на товар {string.Join(" ",marketplaceName)}", 
            replyMarkup: new ForceReplyMarkup(), 
            cancellationToken: cancellationToken);
    }

    private InlineKeyboardMarkup GetInlineKeyboardMarkup()
    {
        var places = MarketPlace.GetAvailableMarketPlaces;
        var keyboards = places
            .Select(t => new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(t, $"/add {t}"),
            })
            .ToList();

        return new InlineKeyboardMarkup(keyboards);
    }
}