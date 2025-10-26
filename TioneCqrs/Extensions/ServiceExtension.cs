using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TioneCqrs.Commands;
using TioneCqrs.Configuration;
using TioneCqrs.Queries;
using TioneCqrs.Services;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class ServiceExtension
    {
        private static readonly Type[] _acceptedCommandHandlersTypes;
        private static readonly Type[] _acceptedQueryHandlersTypes;

        static ServiceExtension()
        {
            _acceptedCommandHandlersTypes = new[] { typeof(ICommandHandler<>), typeof(ICommandHandler<,>) };
            _acceptedQueryHandlersTypes = new[] { typeof(IQueryHandler<,>) };
        }

        /// <summary>
        /// Reqister queries with the specified assembly. Dependency lifetime is set to Scoped by default.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterQueries(this IServiceCollection services, Assembly assembly)
            => RegisterQueries(services, assembly, config => { });

        /// <summary>
        /// Reqister queries within the specified assembly
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterQueries(this IServiceCollection services, Assembly assembly, Action<CqrsConfigurationBuilder> configuration)
        {
            var configBuilder = CqrsConfigurationBuilder.Create();
            configuration.Invoke(configBuilder);
            var config = configBuilder.Build();

            var queryHandlerTypes = GetQueryHandlerTypes(assembly);

            foreach (var handlerType in queryHandlerTypes)
            {
                var interfaceType = handlerType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && _acceptedQueryHandlersTypes.Contains(i.GetGenericTypeDefinition()));

                if (interfaceType is null)
                    continue;

                services.TryAdd(new ServiceDescriptor(interfaceType, handlerType, config.QueriesLifetime.DependenciesLifeteime));
            }

            services.AddQueryDispatcher(config);

            return services;
        }

        /// <summary>
        /// Reqister queries dispatcher. Dependency lifetime is set to Scoped by default.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
            => AddQueryDispatcher(services, new CqrsConfiguration());

        /// <summary>
        /// Resiter the query dispatcher
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueryDispatcher(this IServiceCollection services, CqrsConfiguration cqrsConfiguration)
        {
            services.TryAddSingleton(cqrsConfiguration);
            services.TryAdd(new ServiceDescriptor(typeof(IQueryDispatcher), typeof(QueryDispatcher), cqrsConfiguration.QueriesLifetime.DispatcherLifetime));

            return services;
        }

        private static IEnumerable<Type> GetQueryHandlerTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType && _acceptedQueryHandlersTypes.Contains(i.GetGenericTypeDefinition())));
        }

        /// <summary>
        /// Reqister commands within the specified assembly. Dependency lifetime is set to Scoped by default.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterCommands(this IServiceCollection services, Assembly assembly)
            => RegisterCommands(services, assembly, config => { });

        /// <summary>
        /// Register commands in the specified assembly
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterCommands(this IServiceCollection services, Assembly assembly, Action<CqrsConfigurationBuilder> configuration)
        {
            var configBuilder = CqrsConfigurationBuilder.Create();
            configuration.Invoke(configBuilder);
            var config = configBuilder.Build();
            var commandsHandlerTypes = GetCommandsHandlerTypes(assembly);

            foreach (var handlerType in commandsHandlerTypes)
            {
                var interfaceType = handlerType.GetInterfaces()
                    .First(i => i.IsGenericType && _acceptedCommandHandlersTypes.Contains(i.GetGenericTypeDefinition()));

                services.TryAdd(new ServiceDescriptor(interfaceType, handlerType, config.CommandsLifetime.DependenciesLifeteime));
            }

            services.AddCommandDispatcher(config);

            return services;
        }

        /// <summary>
        /// Reqister commands dispatcher. Dependency lifetime is set to Scoped by default.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services)
            => AddCommandDispatcher(services, new CqrsConfiguration());

        /// <summary>
        /// Register the command dispatcher
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cqrsConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services, CqrsConfiguration cqrsConfiguration)
        {
            services.TryAddSingleton(cqrsConfiguration);
            services.TryAdd(new ServiceDescriptor(typeof(ICommandDispatcher), typeof(CommandDispatcher), cqrsConfiguration.CommandsLifetime.DispatcherLifetime));

            return services;
        }

        private static IEnumerable<Type> GetCommandsHandlerTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType && _acceptedCommandHandlersTypes.Contains(i.GetGenericTypeDefinition())));
        }
    }
}