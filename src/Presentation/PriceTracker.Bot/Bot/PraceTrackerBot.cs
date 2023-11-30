using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PriceTracker.Bot.Bot.Factory;
using PriceTracker.Bot.Options;
using PriceTracker.Domain.Entities;
using PriceTracker.Infrastructure.Context;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PriceTracker.Bot.Bot;

public class PriceTrackerBot
{
    private readonly TelegramBotSettings _settings;
    private readonly TelegramBotClient _client;
    private readonly ICommandHandlerFactory _factory;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    
    public PriceTrackerBot(IOptions<TelegramBotSettings> optionsSnapshot,
        ICommandHandlerFactory factory, 
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _factory = factory;
        _contextFactory = contextFactory;
        _settings = optionsSnapshot.Value;
        _client = new TelegramBotClient(_settings.Token);

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
        var link = message.Text;
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var user = context.Users.FirstOrDefault(x => x.Id.Equals(message.From!.Id));
        var marketPlaceName = replyToMessage!.Split()[^1];
        var site = new Site(marketPlaceName, link);
        user.Sites.Add(site);
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

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
        
        if (callbackQuery.Message is not { } message)
            return;
        
        await botClient.SendTextMessageAsync(
            message.Chat.Id,
            $"Пожалуйста, вставьте ссылку на товар {callbackQuery.Data}", 
            replyMarkup: new ForceReplyMarkup(), 
            cancellationToken: cancellationToken);
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

        Console.WriteLine(errorMessage);
    }
}