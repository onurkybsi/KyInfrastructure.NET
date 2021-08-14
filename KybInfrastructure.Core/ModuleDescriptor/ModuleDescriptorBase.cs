using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// Generic abstract IModuleDescriptor base implementation
    /// </summary>
    public abstract class ModuleDescriptorBase<TModule, TModuleContext> : IModuleDescriptor
    {
        protected readonly TModuleContext _moduleContext;

        public ModuleDescriptorBase(TModuleContext moduleContext)
        {
            _moduleContext = moduleContext;
        }

        /// <summary>
        /// Returns the module's service descriptors
        /// </summary>
        /// <returns>List of service descriptiors</returns>
        public abstract List<ServiceDescriptor> GetDescriptors();

        /// <summary>
        /// Adds all public interfaces of the module to the IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection manipulated with the added service definitions of the module</returns>
        public IServiceCollection Describe(IServiceCollection services)
        {
            GetDescriptors()?
                .ForEach(description => services.Add(description));

            return services;
        }
    }
}