using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using TioneCqrs.Commands;
using TioneCqrs.Queries;
using TioneCqrs.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtension
{
    private static readonly Type[] _acceptedCommandHandlersTypes;
    private static readonly Type[] _acceptedQueryHandlersTypes;

    static ServiceExtension()
    {
        _acceptedCommandHandlersTypes = new[] { typeof(ICommandHandler<>), typeof(ICommandHandler<,>) };
        _acceptedQueryHandlersTypes = new[] { typeof(IQueryHandler<,>) };
    }

    public static IServiceCollection RegisterQueries(this IServiceCollection services, Assembly assembly)
    {
        var queryHandlerTypes = GetQueryHandlerTypes(assembly);

        foreach (var handlerType in queryHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && _acceptedQueryHandlersTypes.Contains(i.GetGenericTypeDefinition()));

            services.TryAddScoped(interfaceType, handlerType);
        }

        services.AddQueryDispatcher();

        return services;
    }

    public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
    {
        services.TryAddScoped<IQueryDispatcher, QueryDispatcher>();

        return services;
    }

    private static IEnumerable<Type> GetQueryHandlerTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && _acceptedQueryHandlersTypes.Contains(i.GetGenericTypeDefinition())));
    }

    public static IServiceCollection RegisterCommands(this IServiceCollection services, Assembly assembly)
    {
        var commandsHandlerTypes = GetCommandsHandlerTypes(assembly);

        foreach (var handlerType in commandsHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && _acceptedCommandHandlersTypes.Contains(i.GetGenericTypeDefinition()));

            services.TryAddScoped(interfaceType, handlerType);
        }

        services.AddCommandDispatcher();

        return services;
    }

    public static IServiceCollection AddCommandDispatcher(this IServiceCollection services)
    {
        services.TryAddScoped<ICommandDispatcher, CommandDispatcher>();

        return services;
    }

    private static IEnumerable<Type> GetCommandsHandlerTypes(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && _acceptedCommandHandlersTypes.Contains(i.GetGenericTypeDefinition())));
    }
}