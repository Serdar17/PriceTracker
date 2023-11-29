using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PriceTracker.Infrastructure.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = PriceTracker.Domain.Entities.User;

namespace PriceTracker.Bot.Bot.Commands;

public class StartCommandHandler : ICommandHandler
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly IMapper _mapper;

    public StartCommandHandler(IDbContextFactory<AppDbContext> factory, IMapper mapper)
    {
        _factory = factory;
        _mapper = mapper;
    }

    public async Task HandleAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken = default)
    {
        if (update.Message is not { } message)
            return;

        await CreateUserIfNotExistsAsync(message, cancellationToken);

        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "PriceTracker - бот для отслеживания цен на популярных маркетплейсах. " +
                  "Для добавления своего товара, нажмите на /add, выберите доступный маркетплей и вставьте ссылку",
            cancellationToken: cancellationToken);
    }

    private async Task CreateUserIfNotExistsAsync(Message message, CancellationToken cancellationToken = default)
    {
        var currentUser = message.From;
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);
        var existUser = context.Users.FirstOrDefault(x => x.Id.Equals(currentUser!.Id));
        if (existUser is null)
        {
            var user = _mapper.Map<User>(currentUser);
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}