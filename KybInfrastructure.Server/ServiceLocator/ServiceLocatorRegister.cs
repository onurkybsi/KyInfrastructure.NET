using Microsoft.Extensions.DependencyInjection;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains ServiceLocator registration strategies to IServiceCollection
    /// </summary>
    public static class ServiceLocatorRegister
    {
        /// <summary>
        /// Initializes the ServiceLocator
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceLocator(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IServiceProviderProxy, HttpContextRequestServicesProxy>();

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                ServiceLocator.Init(serviceProvider.GetRequiredService<IServiceProviderProxy>());

            return services;
        }
    }
}