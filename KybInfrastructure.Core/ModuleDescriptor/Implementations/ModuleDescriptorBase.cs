using KybInfrastructure.Core.UtilityExceptions;
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
        private readonly List<Type> _serviceTypesThatMustBeDescribed;

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

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        /// <param name="serviceTypesThatMustBeDescribed">External service types that are used by this module</param>
        protected ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors, List<Type> serviceTypesThatMustBeDescribed)
        {
            ValidateServiceDescriptors(serviceDescriptors);
            ValidateServiceTypesThatMustBeDescribed(serviceTypesThatMustBeDescribed);

            _serviceDescriptors = serviceDescriptors;
            _moduleContext = default;
            _serviceTypesThatMustBeDescribed = serviceTypesThatMustBeDescribed;
        }

        /// <summary>
        /// IModuleDescriptor base implementation
        /// </summary>
        /// <param name="serviceDescriptors">Module's services descriptors</param>
        /// <param name="moduleContext">Module's context</param>
        /// <param name="serviceTypesThatMustBeDescribed">External service types that are used by this module</param>
        protected ModuleDescriptorBase(List<ServiceDescriptor> serviceDescriptors, TModuleContext moduleContext,
            List<Type> serviceTypesThatMustBeDescribed)
        {
            ValidateServiceDescriptors(serviceDescriptors);
            ValidateModuleContext(moduleContext);
            ValidateServiceTypesThatMustBeDescribed(serviceTypesThatMustBeDescribed);

            _serviceDescriptors = serviceDescriptors;
            _moduleContext = moduleContext;
            _serviceTypesThatMustBeDescribed = serviceTypesThatMustBeDescribed;
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

        private static void ValidateServiceTypesThatMustBeDescribed(List<Type> serviceTypesThatMustBeDescribed)
        {
            if (serviceTypesThatMustBeDescribed is null)
                throw new ArgumentNullException(nameof(serviceTypesThatMustBeDescribed));
            if (serviceTypesThatMustBeDescribed.Any(type => type is null))
                throw new ArgumentNullException("There is a service type which is null in the service types that must be loaded!");
        }

        public List<ServiceDescriptor> GetDescriptors()
            => _serviceDescriptors;

        public IServiceCollection Describe(IServiceCollection services)
        {
            CheckServicesIsRegisteredThatMustBeRegistered(services);

            _serviceDescriptors
                .ForEach(descriptor => services.Add(descriptor));

            return services;
        }

        private void CheckServicesIsRegisteredThatMustBeRegistered(IServiceCollection services)
        {
            if (_serviceTypesThatMustBeDescribed is null)
                return;
            _serviceTypesThatMustBeDescribed.ForEach(type =>
            {
                if (!services.Any(serviceMustBeAdded => serviceMustBeAdded.ServiceType.AssemblyQualifiedName == type.AssemblyQualifiedName))
                    throw new ModuleLoadingException($"Module {this.GetType()} depend on {type}, but in the service collection, it's not exists!");

            });
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

        protected List<Type> GetDependentServiceTypes()
            => _serviceTypesThatMustBeDescribed ?? new();
    }
}