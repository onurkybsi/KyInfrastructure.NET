using KybInfrastructure.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains dependency registration strategies of a module to the server application
    /// </summary>
    public static class ModuleRegister
    {
        /// <summary>
        /// Middleware that adds a module to service collections
        /// </summary>
        /// <typeparam name="TModuleDescriptor">Module descriptor type</typeparam>
        /// <typeparam name="TModuleContext">Module context type</typeparam>
        /// <param name="services"></param>
        /// <param name="context">Context instance of module that use in creation o module descriptor</param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModuleDescriptor, TModuleContext>(this IServiceCollection services, TModuleContext context)
            where TModuleDescriptor : IModuleDescriptor
            where TModuleContext : IModuleContext
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            TModuleDescriptor moduleDescriptor = (TModuleDescriptor)Activator.CreateInstance(typeof(TModuleDescriptor), context);
            moduleDescriptor.Describe(services);

            return services;
        }

        /// <summary>
        /// Middleware that adds a module to service collections
        /// </summary>
        /// <typeparam name="TModuleDescriptor">Module descriptor type</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModuleDescriptor>(this IServiceCollection services)
            where TModuleDescriptor : IModuleDescriptor
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            TModuleDescriptor moduleDescriptor = (TModuleDescriptor)Activator.CreateInstance(typeof(TModuleDescriptor));
            moduleDescriptor.Describe(services);

            return services;
        }
    }
}
