using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TioneCqrs.Exceptions;
using TioneCqrs.Queries;

namespace TioneCqrs.Services;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueryDispatcher>? _logger;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetService<ILogger<QueryDispatcher>>();
    }

    public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var queryType = query.GetType();

        using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var handler = scope.ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();
            if (handler is null) throw new InvalidQueryException(queryType);

            _logger?.LogDebug($"Executing query {queryType.FullName}");

            return handler.ExecuteAsync(query, cancellationToken);
        }
    }
}