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
        private readonly TModuleContext _moduleContext;
        private readonly List<ServiceDescriptor> _serviceDescriptors;

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        protected ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors)
        {
            ValidateServiceDescriptors(serviceDescriptors);

            _serviceDescriptors = serviceDescriptors;
            _moduleContext = default;
        }

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        /// <param name="moduleContext">Module's context</param>
        protected ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors, TModuleContext moduleContext)
        {
            ValidateServiceDescriptors(serviceDescriptors);
            ValidateModuleContext(moduleContext);

            _serviceDescriptors = serviceDescriptors;
            _moduleContext = moduleContext;
        }

        private static void ValidateServiceDescriptors(List<ServiceDescriptor> serviceDescriptors)
        {
            if (serviceDescriptors is null)
                throw new ArgumentNullException(nameof(serviceDescriptors));
            if (serviceDescriptors.Any(descriptor => descriptor is null))
                throw new InvalidArgumentException("serviceDescriptor", null);
        }

        private static void ValidateModuleContext(TModuleContext moduleContext)
        {
            if (moduleContext is null)
                throw new ArgumentNullException(nameof(moduleContext));
        }

        public List<ServiceDescriptor> GetDescriptors()
            => _serviceDescriptors;

        public IServiceCollection Describe(IServiceCollection services)
        {
            _serviceDescriptors
                .ForEach(descriptor => services.Add(descriptor));

            return services;
        }

        /// <summary>
        /// Returns context of the module
        /// </summary>
        /// <returns>Module context</returns>
        protected TModuleContext GetContext()
        {
            if (_moduleContext is null)
                throw new InvalidOperationException("moduleContext is null");
            else
                return _moduleContext;
        }
    }
}