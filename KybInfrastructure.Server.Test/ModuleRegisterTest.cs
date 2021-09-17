using KybInfrastructure.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class ModuleRegisterTest
    {
        private class FakeModuleDescriptor : ModuleDescriptorBase<IModuleContext>
        {
            public FakeModuleDescriptor()
                : base(new List<ServiceDescriptor>
                {
                    ServiceDescriptor.Singleton(typeof(IServiceCollection), typeof(ServiceCollection))
                })
            { }

            public FakeModuleDescriptor(IModuleContext moduleContext)
                : base(new List<ServiceDescriptor>
                {
                    ServiceDescriptor.Singleton(typeof(IServiceCollection), typeof(ServiceCollection))
                })
            {
                throw new Exception("Fake exception");
            }

            public IServiceCollection Describe(IServiceCollection services)
                => throw new NotImplementedException();

            public List<ServiceDescriptor> GetDescriptors()
                => throw new NotImplementedException();
        }

        [Fact]
        public void AddModule_Throws_ArgumentNullException_If_Given_ModuleContext_Is_Null()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>(() => serviceCollection.AddModule<FakeModuleDescriptor, IModuleContext>(null));
        }

        [Fact]
        public void AddModule_Adds_Descriptor_To_ServiceCollection()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddModule<FakeModuleDescriptor>();

            Assert.Contains(serviceCollection, service => service.ImplementationType == typeof(ServiceCollection));
        }

        [Fact]
        public void AddModule_Adds_All_Descriptors_To_ServiceCollection()
        {
            int allDescriptorsCount = 1;
            ServiceCollection serviceCollection = new ServiceCollection();
            
            serviceCollection.AddModule<FakeModuleDescriptor>();

            Assert.Equal(allDescriptorsCount, serviceCollection.Count);
        }

        [Fact]
        public void AddModule_Throws_InvalidOperationException_If_Module_Couldnt_Construct()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            Mock<IModuleContext> fakeContext = new Mock<IModuleContext>();

            Assert.Throws<InvalidOperationException>(() => 
                serviceCollection.AddModule<FakeModuleDescriptor, IModuleContext>(fakeContext.Object));
        }
    }
}
