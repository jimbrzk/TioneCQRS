namespace TioneCqrs.Queries;

public interface IQueryDispatcher
{
    /// <summary>
    /// Executes given query and returns it's result
    /// </summary>
    /// <typeparam name="TQuery">TioneCQTS query type</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="query">TioneCQRS query</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exceptions.InvalidQueryException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery;
}