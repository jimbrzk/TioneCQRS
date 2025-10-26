using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TioneCqrs.Commands;
using TioneCqrs.Configuration;
using TioneCqrs.Exceptions;

namespace TioneCqrs.Services
{

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CqrsConfiguration _configuration;
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = _serviceProvider.GetRequiredService<CqrsConfiguration>();
            if (_configuration.LoggingEnabled)
                _logger = _serviceProvider.GetService<ILogger<CommandDispatcher>>();
        }

        public Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            var handler = _serviceProvider.GetService<ICommandHandler<TCommand, TResult>>();
            if (handler is null) throw new InvalidCommandException(commandType);

            _logger?.LogDebug($"Executing command {commandType.FullName}");

            return handler.ExecuteAsync(command, cancellationToken);
        }

        public Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
            if (handler is null) throw new InvalidCommandException(commandType);

            _logger?.LogDebug($"Executing command {commandType.FullName}");

            return handler.ExecuteAsync(command, cancellationToken);
        }
    }
}