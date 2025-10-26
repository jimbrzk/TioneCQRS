using Microsoft.Extensions.DependencyInjection;

namespace TioneCqrs.Configuration
{
#if NETSTANDARD2_0_OR_GREATER
    public class CqrsLifetimeConfiguration
    {
        public ServiceLifetime DependenciesLifeteime { get; internal set; } = ServiceLifetime.Scoped;
        public ServiceLifetime DispatcherLifetime { get; internal set; } = ServiceLifetime.Scoped;
    }
#elif NET8_0_OR_GREATER
    public record CqrsLifetimeConfiguration
    {
        public ServiceLifetime DependenciesLifeteime { get; init; } = ServiceLifetime.Scoped;
        public ServiceLifetime DispatcherLifetime { get; init; } = ServiceLifetime.Scoped;
    }
#endif
}