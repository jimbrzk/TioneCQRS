using System;

namespace TioneCqrs.Exceptions
{

    /// <summary>
    /// Invalid command was called. It can be invalid TioneCQRS query type or it's not registred in Dependency Injection services collection
    /// </summary>
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException(Type queryType) : base("Invalid query was called. It can be invalid TioneCQRS query type or it's not registred in Dependency Injection services collection")
        {
            QueryType = queryType;
        }

        public readonly Type QueryType;
    }
}