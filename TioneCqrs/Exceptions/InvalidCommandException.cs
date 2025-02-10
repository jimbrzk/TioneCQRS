namespace TioneCqrs.Exceptions;

/// <summary>
/// Invalid command was called. It can be invalid TioneCQRS command type or it's not registred in Dependency Injection services collection
/// </summary>
public class InvalidCommandException(Type commandType) : Exception("Invalid command was called. It can be invalid TioneCQRS command type or it's not registred in Dependency Injection services collection")
{
    public Type CommandType { get; } = commandType;
}