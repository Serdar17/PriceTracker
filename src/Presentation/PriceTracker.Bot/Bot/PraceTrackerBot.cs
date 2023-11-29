using Microsoft.Extensions.Options;
using PriceTracker.Bot.Options;
using PriceTracker.Common.Constants;
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
    
    public PriceTrackerBot(IOptions<TelegramBotSettings> optionsSnapshot)
    {
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
        {
            await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
        }
        
        if (update.Message is not { } message)
            return;
        
        if (message.Text is not { } messageText)
            return;
        
        var user = message.From;
        Console.WriteLine("Message from user = " + message.From);
        
        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        if (messageText.ToLower().Equals(MenuCommands.Add))
        { 
            var keyboards = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ozon"),
                },
            });

            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите доступный сайт для парсинга цены",
                replyMarkup: keyboards,
                cancellationToken: cancellationToken);
        }
        
        // Message sentMessage = await botClient.SendTextMessageAsync(
        //     chatId: chatId,
        //     text: "You said:\n" + messageText,
        //     cancellationToken: cancellationToken);
        
        // var sentMessage = await botClient.SendTextMessageAsync(
        //     chatId: chatId,
        //     text: "Trying *all the parameters* of `sendMessage` method",
        //     parseMode: ParseMode.MarkdownV2,
        //     disableNotification: true,
        //     replyToMessageId: update.Message.MessageId,
        //     replyMarkup: new InlineKeyboardMarkup(
        //         InlineKeyboardButton.WithUrl(
        //             text: "Check sendMessage method",
        //             url: "https://core.telegram.org/bots/api#sendmessage")
        //         ),
        //     cancellationToken: cancellationToken);
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
        
        // var sentMessage = await botClient.SendTextMessageAsync(
        //     chatId: message.Chat.Id,
        //     text: "Вставьте ссылку на товар",
        //     cancellationToken: cancellationToken);
        
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