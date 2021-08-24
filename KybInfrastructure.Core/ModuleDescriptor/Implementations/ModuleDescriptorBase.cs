using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// Abstract IModuleDescriptor base implementation
    /// </summary>
    public abstract class ModuleDescriptorBase<TModuleContext> : IModuleDescriptor
        where TModuleContext : IModuleContext
    {
        protected readonly TModuleContext ModuleContext;

        protected ModuleDescriptorBase(TModuleContext moduleContext)
        {
            ModuleContext = moduleContext;
        }

        protected ModuleDescriptorBase() { }

        public abstract List<ServiceDescriptor> GetDescriptors();

        public IServiceCollection Describe(IServiceCollection services)
        {
            GetDescriptors()?
                .ForEach(description => services.Add(description));

            return services;
        }
    }
}