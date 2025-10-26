using Microsoft.Extensions.DependencyInjection;

namespace TioneCqrs.Configuration
{
#if NETSTANDARD2_0_OR_GREATER
    public class CqrsConfiguration
    {
        public CqrsLifetimeConfiguration CommandsLifetime { get; internal set; } = new CqrsLifetimeConfiguration();
        public CqrsLifetimeConfiguration QueriesLifetime { get; internal set; } = new CqrsLifetimeConfiguration();
        public bool LoggingEnabled { get; internal set; } = true;
    }
#elif NET8_0_OR_GREATER
    public record CqrsConfiguration
    {
        public CqrsLifetimeConfiguration CommandsLifetime { get; init; } = new();
        public CqrsLifetimeConfiguration QueriesLifetime { get; init; } = new();
        public bool LoggingEnabled { get; init; } = true;
    }
#endif
}