using PriceTracker.Domain.Entities;
using PriceTracker.Services.Product;
using PriceTracker.Services.User;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PriceTracker.Commands.Commands;

public class RemoveCommandHandler : ICommandHandler
{
    private readonly IUserService _userService;
    private readonly IProductService _productService;

    public RemoveCommandHandler(IUserService userService, 
        IProductService productService)
    {
        _userService = userService;
        _productService = productService;
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
            text: "\ud83d\uddd1 Чтобы удалить товар из списка отслеживаемых, просто нажмите на него!",
            replyMarkup: keyboards,
            cancellationToken: cancellationToken);
    }

    public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        if (callbackQuery.Data is null)
            return;
        
        if (callbackQuery.Message is not { } message)
            return;
        
        if (callbackQuery.From is not { } user)
            return;

        if (int.TryParse(callbackQuery.Data.Split()[^1], out var productId))
        {
            await _productService.RemoveProductById(productId, cancellationToken);    
        }
        var products = await _userService.GetProductsByUserIdAsync(user.Id, cancellationToken);
        var keyboards = GetInlineKeyboardMarkup(products);
        
        await botClient.EditMessageReplyMarkupAsync(
            callbackQuery.Message.Chat.Id, 
            callbackQuery.Message.MessageId,
            keyboards, cancellationToken: cancellationToken);
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "\u2705 Товар успешно удален!",
            cancellationToken: cancellationToken);
    }

    private InlineKeyboardMarkup GetInlineKeyboardMarkup(IEnumerable<Product> products)
    {
        var keyboards = products
            .Select((p, i) => new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(p.Link, $"/remove {p.Id}")
            })
            .ToList();

        return new InlineKeyboardMarkup(keyboards);
    } 
}