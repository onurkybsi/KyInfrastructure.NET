using KybInfrastructure.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void AddModule_Adds_All_Descriptors_To_ServiceCollection()
        {
            int allDescriptorsCount = 1;
            ServiceCollection serviceCollection = new ServiceCollection();
            
            serviceCollection.AddModule<FakeModuleDescriptor>();

            Assert.Equal(allDescriptorsCount, serviceCollection.Count);
        }
    }
}
