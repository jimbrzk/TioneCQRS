using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands;

public class TestCommandWithoutResultHandler : ICommandHandler<TestCommandWithoutResult>
{

    public Task ExecuteAsync(TestCommandWithoutResult command, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}