using KybInfrastructure.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains registration strategies of a module to the server application via IModuleDescriptor
    /// </summary>
    public static class ModuleRegister
    {
        /// <summary>
        /// Adds service descriptions of the module to service collections given
        /// </summary>
        /// <typeparam name="TModuleDescriptor">Module descriptor type</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModuleDescriptor>(this IServiceCollection services)
            where TModuleDescriptor : class, IModuleDescriptor
        {
            try
            {
                TModuleDescriptor moduleDescriptor = (TModuleDescriptor)Activator.CreateInstance(typeof(TModuleDescriptor));
                moduleDescriptor.Describe(services);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"IModuleDescriptor couldn't construct: {ex}");
            }

            return services;
        }

        /// <summary>
        /// Adds service descriptions of the module to service collections given
        /// </summary>
        /// <typeparam name="TModuleDescriptor">Module descriptor type</typeparam>
        /// <typeparam name="TModuleContext">Module context type</typeparam>
        /// <param name="services"></param>
        /// <param name="context">Context instance of the module that to be used in creation of the module descriptor</param>
        /// <returns></returns>
        public static IServiceCollection AddModule<TModuleDescriptor, TModuleContext>(this IServiceCollection services, TModuleContext context)
                where TModuleDescriptor : class, IModuleDescriptor
                where TModuleContext : IModuleContext
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                TModuleDescriptor moduleDescriptor = (TModuleDescriptor)Activator.CreateInstance(typeof(TModuleDescriptor), context);
                moduleDescriptor.Describe(services);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"IModuleDescriptor couldn't construct: {ex}");
            }

            return services;
        }
    }
}