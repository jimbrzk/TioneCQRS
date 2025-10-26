using Microsoft.Extensions.DependencyInjection;

namespace TioneCqrs.Configuration
{
    public class CqrsConfigurationBuilder
    {
#if NETSTANDARD2_0_OR_GREATER
        private CqrsConfiguration _current = new CqrsConfiguration();
#elif NET8_0_OR_GREATER
        private CqrsConfiguration _current = new();
#endif

        public static CqrsConfigurationBuilder Create()
        {
            return new CqrsConfigurationBuilder();
        }

        public CqrsConfigurationBuilder WithQueriesLifetime(ServiceLifetime dispatcher, ServiceLifetime queries)
        {
#if NETSTANDARD2_0_OR_GREATER
            _current.QueriesLifetime.DispatcherLifetime = dispatcher;
            _current.QueriesLifetime.DependenciesLifeteime = queries;
#elif NET8_0_OR_GREATER
            _current = _current with
            {
                QueriesLifetime = new CqrsLifetimeConfiguration()
                {
                    DispatcherLifetime = dispatcher,
                    DependenciesLifeteime = queries
                }
            };
#endif
            return this;
        }

        public CqrsConfigurationBuilder WithCommandsLifetime(ServiceLifetime dispatcher, ServiceLifetime commands)
        {
#if NETSTANDARD2_0_OR_GREATER
            _current.CommandsLifetime.DispatcherLifetime = dispatcher;
            _current.CommandsLifetime.DependenciesLifeteime = commands;
#elif NET8_0_OR_GREATER
            _current = _current with
            {
                CommandsLifetime = new CqrsLifetimeConfiguration()
                {
                    DispatcherLifetime = dispatcher,
                    DependenciesLifeteime = commands
                }
            };
#endif
            return this;
        }

        public CqrsConfigurationBuilder DisableLogging()
        {
#if NETSTANDARD2_0_OR_GREATER
            _current.LoggingEnabled = false;
#elif NET8_0_OR_GREATER
            _current = _current with { LoggingEnabled = false };
#endif
            return this;
        }

#if NETSTANDARD2_0_OR_GREATER
        public CqrsConfiguration Build()
            => _current;
#elif NET8_0_OR_GREATER
        public CqrsConfiguration Build()
            => _current;
#endif
    }
}
