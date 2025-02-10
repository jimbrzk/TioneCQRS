using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands;

public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, string>
{
    public Task<string> ExecuteAsync(TestCommandWithResult command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(command.Test);
    }
}