using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// Provides a module to be defined to another module via IServiceCollection container
    /// </summary>
    public interface IModuleDescriptor
    {
        /// <summary>
        /// Returns the ServiceDescriptor of all public interfaces of the module
        /// </summary>
        /// <returns>Service descriptions of all public interfaces</returns>
        List<ServiceDescriptor> GetDescriptions();

        /// <summary>
        /// Adds all public interfaces of the module to the IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection manipulated with the added service definitions of the module</returns>
        IServiceCollection Describe(IServiceCollection services);
    }
}