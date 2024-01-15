using AutoMapper;
using PriceTracker.Services.User;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = PriceTracker.Domain.Entities.User;

namespace PriceTracker.Commands.Commands;

public class StartCommandHandler : ICommandHandler
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public StartCommandHandler(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken = default)
    {
        if (update.Message is not { } message)
            return;

        var user = _mapper.Map<User>(message.From);
        user.ChatId = message.Chat.Id;
        await _userService.CreateUserAsync(user, cancellationToken);
        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "\ud83d\uded2 PriceTracker - бот для удобного отслеживания цен на популярных маркетплейсах. " +
                  "Выберите в меню /add, выберите интересующий маркетплейс и вставьте ссылку на товар - и ваша ценовая " +
                  "магия начнется! \ud83d\ude80",
            cancellationToken: cancellationToken);
    }

    public Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}