namespace TioneCqrs.Commands;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    Task<TResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
}