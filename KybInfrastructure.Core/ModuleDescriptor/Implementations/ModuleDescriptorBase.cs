using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// IModuleDescriptor base implementation
    /// </summary>
    public class ModuleDescriptorBase<TModuleContext> : IModuleDescriptor
        where TModuleContext : IModuleContext
    {
        protected readonly List<ServiceDescriptor> ServiceDescriptors;
        protected readonly TModuleContext ModuleContext;

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        public ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors)
        {
            ValidateServiceDescriptors(serviceDescriptors);

            ServiceDescriptors = serviceDescriptors;
            ModuleContext = default;
        }

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        /// <param name="moduleContext">Module's context</param>
        public ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors, TModuleContext moduleContext)
        {
            ValidateServiceDescriptors(serviceDescriptors);
            ValidateModuleContext(moduleContext);

            ServiceDescriptors = serviceDescriptors;
            ModuleContext = moduleContext;
        }

        private static void ValidateServiceDescriptors(List<ServiceDescriptor> serviceDescriptors)
        {
            if (serviceDescriptors is null)
                throw new ArgumentNullException(nameof(serviceDescriptors));
            if (serviceDescriptors.Any(descriptor => descriptor is null))
                throw new ArgumentNullException("serviceDescriptor");
        }

        private static void ValidateModuleContext(TModuleContext moduleContext)
        {
            if (moduleContext is null)
                throw new ArgumentNullException(nameof(moduleContext));
        }

        public List<ServiceDescriptor> GetDescriptors()
            => ServiceDescriptors;

        public IServiceCollection Describe(IServiceCollection services)
        {
            GetDescriptors()?
                .ForEach(description => services.Add(description));

            return services;
        }
    }
}