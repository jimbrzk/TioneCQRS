using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands;

public record TestCommandWithResult(string Test) : ICommand;