using PriceTracker.Bot.Bot.Commands;

namespace PriceTracker.Bot.Bot.Factory;

public interface ICommandHandlerFactory
{
    ICommandHandler? CreateHandler(string command);
}