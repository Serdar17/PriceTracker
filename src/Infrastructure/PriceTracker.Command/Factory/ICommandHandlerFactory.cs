using PriceTracker.Commands.Commands;

namespace PriceTracker.Commands.Factory;

public interface ICommandHandlerFactory
{
    ICommandHandler? CreateHandler(string command);
}