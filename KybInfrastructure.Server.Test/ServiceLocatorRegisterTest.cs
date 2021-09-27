using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class ServiceLocatorRegisterTest
    {
        [Fact]
        public void AddServiceLocator_Registers_IHttpContextAccessor_To_ServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceLocator();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            Assert.NotNull(httpContextAccessor);
        }

        [Fact]
        public void AddServiceLocator_Registers_IServiceProviderProxy_To_ServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceLocator();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IServiceProviderProxy serviceProviderProxy = serviceProvider.GetRequiredService<IServiceProviderProxy>();

            Assert.NotNull(serviceProviderProxy);
        }

        [Fact]
        public void AddServiceLocator_Registers_IServiceProviderProxy_To_ServiceCollection_AsSingleton()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceLocator();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IServiceProviderProxy serviceProviderProxy1;
            using (IServiceScope serviceScope1 = serviceProvider.CreateScope())
            {
                serviceProviderProxy1 = serviceScope1.ServiceProvider.GetRequiredService<IServiceProviderProxy>();
            }
            IServiceProviderProxy serviceProviderProxy2;
            using (IServiceScope serviceScope2 = serviceProvider.CreateScope())
            {
                serviceProviderProxy2 = serviceScope2.ServiceProvider.GetRequiredService<IServiceProviderProxy>();
            }

            Assert.Equal(serviceProviderProxy1, serviceProviderProxy2);
        }

        [Fact]
        public void AddServiceLocator_Initializes_ServiceLocator()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceLocator();
            _ = services.BuildServiceProvider();

            Assert.NotNull(ServiceLocator.Instance);
        }
    }
}
