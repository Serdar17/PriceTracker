using PriceTracker.Bot.Bot.Commands;
using PriceTracker.Common.Constants;

namespace PriceTracker.Bot.Bot.Factory;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IServiceScope _scope;
    private readonly IEnumerable<ICommandHandler> _commands;

    public CommandHandlerFactory(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _scope = provider.CreateScope();
        _commands = _scope.ServiceProvider.GetServices<ICommandHandler>();
    }

    public ICommandHandler? CreateHandler(string command)
    {
        switch (command)
        {
            case MenuCommands.Start:
                return _commands.First(x => x is StartCommandHandler);
            case MenuCommands.Add:
                return _commands.First(x => x is AddCommandHandler);
            default:
                return null;
        }
    }
}