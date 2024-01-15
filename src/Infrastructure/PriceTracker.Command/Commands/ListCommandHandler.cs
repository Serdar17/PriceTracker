using PriceTracker.Domain.Entities;
using PriceTracker.Services.User;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PriceTracker.Commands.Commands;

public class ListCommandHandler : ICommandHandler
{
    private readonly IUserService _userService;

    public ListCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task HandleAsync(ITelegramBotClient botClient,
        Update update, 
        CancellationToken cancellationToken = default)
    {
        if (update.Message is not { } message)
            return;

        if (message.From is not { } user)
            return;
        
        var products = await _userService.GetProductsByUserIdAsync(user.Id, cancellationToken);
        var keyboards = GetInlineKeyboardMarkup(products);
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "\ud83d\udccb Ваш список товаров:",
            replyMarkup: keyboards,
            cancellationToken: cancellationToken);
    }

    public Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    private InlineKeyboardMarkup GetInlineKeyboardMarkup(IEnumerable<Product> products)
    {
        var keyboards = products
            .Select((p, i) => new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithUrl(p.Title, p.Link)
            })
            .ToList();

        return new InlineKeyboardMarkup(keyboards);
    } 
}