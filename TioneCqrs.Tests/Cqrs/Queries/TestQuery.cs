using TioneCqrs.Queries;

namespace TioneCqrs.Tests.Cqrs.Queries;

public record TestQuery(string Test) : IQuery;