using Microsoft.Extensions.Options;
using PriceTracker.Bot.Options;
using PriceTracker.Commands.Factory;
using PriceTracker.Domain.Entities;
using PriceTracker.Domain.Telegram;
using PriceTracker.Services.Parser.Factory;
using PriceTracker.Services.User;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PriceTracker.Bot.Bot;

public class PriceTrackerBot : ITelegramClient
{
    private readonly TelegramBotClient _client;
    private readonly ICommandHandlerFactory _factory;
    private readonly ILogger<PriceTrackerBot> _logger;
    private readonly IUserService _userService;
    private readonly IParserFactory _parserFactory;
    
    public PriceTrackerBot(IOptions<TelegramBotSettings> optionsSnapshot,
        ICommandHandlerFactory factory, 
        ILogger<PriceTrackerBot> logger, 
        IUserService userService, 
        IParserFactory parserFactory)
    {
        _factory = factory;
        _logger = logger;
        _userService = userService;
        _parserFactory = parserFactory;
        _client = new TelegramBotClient(optionsSnapshot.Value.Token);

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        using CancellationTokenSource cts = new ();
        
       _client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient, 
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.CallbackQuery is not null)
            await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
        
        if (update.Message is not { } message)
            return;
        
        if (message.Text is not { } messageText)
            return;

        if (message.ReplyToMessage is not null)
            await HandleReplyToMessageAsync(message, cancellationToken);
        
        var command = _factory.CreateHandler(messageText.ToLower());

        if (command is not null)
            await command.HandleAsync(botClient, update, cancellationToken);
    }

    private async Task HandleReplyToMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var replyToMessage = message.ReplyToMessage!.Text;
        var marketPlaceName = string.Join(" ", replyToMessage!.Split().Skip(5));
        var parser = _parserFactory.CreateParser(marketPlaceName);
        if (parser is not null && message.Text is not null)
        {
            try
            {
                var parseResult = await parser.ParseAsync(message.Text);
                if (parseResult.Title is not null)
                {
                    var product = new Product(marketPlaceName, parseResult.Title, message.Text);
                    var price = new Price(parseResult.Price ?? 0.0, parseResult.CardPrice ?? 0.0);
                    product.Prices.Add(price);
                    await _userService.AddProductToUserAsync(message.From.Id, product, cancellationToken);
            
                    await _client.SendTextMessageAsync(
                        message.Chat.Id,
                        "Товар успешно добавлен, ожидайте изменение цены!\n" +
                        $"Название товара\n *{parseResult.Title}*\nЦена товара: *{parseResult.Price}*\n" +
                        $"Цена товара по скидке/карте: *{parseResult?.CardPrice}*",
                        parseMode: ParseMode.Markdown,
                        cancellationToken: cancellationToken);
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }    
        }
        
        await _client.SendTextMessageAsync(
            message.Chat.Id,
            "Товар не был добавлен, проверьте наличие и доступность товара на маркетплейсе",
            cancellationToken: cancellationToken);
    }

    public async Task SendPriceChangingNotification(long chatId, string message, CancellationToken cancellationToken = default)
    {
       await _client.SendTextMessageAsync(
            chatId,
            message,
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }
    
    private async Task HandleCallbackQueryAsync(
        ITelegramBotClient botClient,
        CallbackQuery callbackQuery,
        CancellationToken cancellationToken)
    {
        if (callbackQuery.Data is null)
            return;

        var commandName = callbackQuery.Data.Split()[0];
        var command = _factory.CreateHandler(commandName.ToLower());

        if (command is not null)
            await command.HandleCallbackQueryAsync(botClient, callbackQuery, cancellationToken);
    }
    
    
    private async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError("Price tracker Bot error {ApiErrorMessage}", errorMessage);
        Thread.Sleep(2000);
    }
}