using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TioneCqrs.Configuration;
using TioneCqrs.Exceptions;
using TioneCqrs.Queries;

namespace TioneCqrs.Services
{

    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CqrsConfiguration _configuration;
#if NETSTANDARD2_0
        private readonly ILogger<QueryDispatcher> _logger;
#elif NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        private readonly ILogger<QueryDispatcher>? _logger;
#endif

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = _serviceProvider.GetRequiredService<CqrsConfiguration>();
#if NETSTANDARD2_0_OR_GREATER
            if (_configuration.LoggingEnabled)
                _logger = _serviceProvider.GetService<ILogger<QueryDispatcher>>();
#elif NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            _logger = (_configuration.LoggingEnabled)
                ? _serviceProvider.GetService<ILogger<QueryDispatcher>>()
                : null;
#endif
        }

        public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryType = query.GetType();

            var handler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();
            if (handler is null) throw new InvalidQueryException(queryType);

            _logger?.LogDebug($"Executing query {queryType.FullName}");

            return handler.ExecuteAsync(query, cancellationToken);
        }
    }
}