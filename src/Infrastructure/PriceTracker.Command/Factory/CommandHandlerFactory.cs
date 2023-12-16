using Microsoft.Extensions.DependencyInjection;
using PriceTracker.Commands.Commands;
using PriceTracker.Common.Constants;

namespace PriceTracker.Commands.Factory;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IEnumerable<ICommandHandler> _commands;

    public CommandHandlerFactory(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _commands = provider.CreateScope().ServiceProvider.GetServices<ICommandHandler>();
    }

    public ICommandHandler? CreateHandler(string command)
    {
        switch (command)
        {
            case MenuCommands.Start:
                return _commands.First(x => x is StartCommandHandler);
            case MenuCommands.Add:
                return _commands.First(x => x is AddCommandHandler);
            case MenuCommands.Remove:
                return _commands.First(x => x is RemoveCommandHandler);
            case MenuCommands.List:
                return _commands.First(x => x is ListCommandHandler);
            default:
                return null;
        }
    }
}