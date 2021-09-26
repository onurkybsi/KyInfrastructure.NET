using Microsoft.Extensions.DependencyInjection;

namespace KybInfrastructure.Server
{
    public static class ServiceLocatorRegister
    {
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