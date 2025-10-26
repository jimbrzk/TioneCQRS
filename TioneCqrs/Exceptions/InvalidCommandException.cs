using System;

namespace TioneCqrs.Exceptions
{

    /// <summary>
    /// Invalid command was called. It can be invalid TioneCQRS command type or it's not registred in Dependency Injection services collection
    /// </summary>
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(Type commandType) : base("Invalid command was called. It can be invalid TioneCQRS command type or it's not registred in Dependency Injection services collection")
        {
            CommandType = commandType;
        }

        public readonly Type CommandType;
    }
}