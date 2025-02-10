using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TioneCqrs.Commands;
using TioneCqrs.Exceptions;

namespace TioneCqrs.Services;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandDispatcher>? _logger;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetService<ILogger<CommandDispatcher>>();
    }

    public Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var commandType = command.GetType();

        using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand, TResult>>();
            if (handler is null) throw new InvalidCommandException(commandType);

            _logger?.LogDebug($"Executing command {commandType.FullName}");

            return handler.ExecuteAsync(command, cancellationToken);
        }
    }

    public Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var commandType = command.GetType();

        using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
            if (handler is null) throw new InvalidCommandException(commandType);

            _logger?.LogDebug($"Executing command {commandType.FullName}");

            return handler.ExecuteAsync(command, cancellationToken);
        }
    }
}