namespace TioneCqrs.Commands;

/// <summary>
/// CQRS Command Dispatcher
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Executes given command and returns it result
    /// </summary>
    /// <typeparam name="TCommand">TioneCQRS command type</typeparam>
    /// <typeparam name="TResult">Command result type</typeparam>
    /// <param name="command">TioneCQRS command</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exceptions.InvalidCommandException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;

    /// <summary>
    /// Executes given command
    /// </summary>
    /// <typeparam name="TCommand">TioneCQRS command type</typeparam>
    /// <param name="command">TioneCQRS command</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exceptions.InvalidCommandException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
}