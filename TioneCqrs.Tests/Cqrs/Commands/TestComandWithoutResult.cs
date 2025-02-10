using TioneCqrs.Commands;

namespace TioneCqrs.Tests.Cqrs.Commands;

public record TestCommandWithoutResult(string Test) : ICommand;