using Microsoft.EntityFrameworkCore;
using PriceTracker.Infrastructure.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PriceTracker.Bot.Bot.Commands;

public class AddCommandHandler : ICommandHandler
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AddCommandHandler(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken = default)
    {
        if (update.Message is not { } message)
            return;
        
        var keyboards = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Ozon"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Wildberries"),
            },
        });

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите доступный сайт для парсинга цены",
            replyMarkup: keyboards,
            cancellationToken: cancellationToken);
    }
}