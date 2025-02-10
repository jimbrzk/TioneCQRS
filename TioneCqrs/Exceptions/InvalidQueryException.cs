namespace TioneCqrs.Exceptions;

/// <summary>
/// Invalid command was called. It can be invalid TioneCQRS query type or it's not registred in Dependency Injection services collection
/// </summary>
public class InvalidQueryException(Type queryType) : Exception("Invalid command was called. It can be invalid TioneCQRS query type or it's not registred in Dependency Injection services collection")
{
    public Type QueryType { get; } = queryType;
}