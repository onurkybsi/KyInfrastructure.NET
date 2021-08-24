using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// Provide methods that integrate a module to another modules via a IServiceCollection
    /// </summary>
    public interface IModuleDescriptor
    {
        /// <summary>
        /// Returns the ServiceDescriptor of all public assets of the module
        /// </summary>
        /// <returns>Service descriptions of all public assets</returns>
        List<ServiceDescriptor> GetDescriptors();

        /// <summary>
        /// Adds all public assests of the module to the IServiceCollection given
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection manipulated with the added service descriptions of the module</returns>
        IServiceCollection Describe(IServiceCollection services);
    }
}