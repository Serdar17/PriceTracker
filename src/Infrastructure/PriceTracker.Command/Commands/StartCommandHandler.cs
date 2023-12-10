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
        await _userService.CreateUserAsync(user, cancellationToken);

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "PriceTracker - бот для отслеживания цен на популярных маркетплейсах. " +
                  "Для добавления своего товара, нажмите на /add, выберите доступный маркетплей и вставьте ссылку",
            cancellationToken: cancellationToken);
    }

    public Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}