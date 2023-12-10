using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PriceTracker.Bot.Options;
using PriceTracker.Commands.Factory;
using PriceTracker.Domain.Entities;
using PriceTracker.Infrastructure.Context;
using PriceTracker.Services.User;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PriceTracker.Bot.Bot;

public class PriceTrackerBot
{
    private readonly TelegramBotClient _client;
    private readonly ICommandHandlerFactory _factory;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ILogger<PriceTrackerBot> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    
    public PriceTrackerBot(IOptions<TelegramBotSettings> optionsSnapshot,
        ICommandHandlerFactory factory, 
        IDbContextFactory<AppDbContext> contextFactory, 
        ILogger<PriceTrackerBot> logger, IMapper mapper, IUserService userService)
    {
        _factory = factory;
        _contextFactory = contextFactory;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        var settings = optionsSnapshot.Value;
        _client = new TelegramBotClient(settings.Token);

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
        var marketPlaceName = replyToMessage!.Split()[^1];
        var product = new Product(marketPlaceName, message.Text);
        await _userService.AddProductToUserAsync(message.From.Id, product, cancellationToken);
        
        await _client.SendTextMessageAsync(
            message.Chat.Id,
            "Товар успешно добавлен, ожидайте изменение цены!",
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
    }
}