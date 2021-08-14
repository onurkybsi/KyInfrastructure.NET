using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace KybInfrastructure.Core.Test
{
    public class ModuleDescriptorTest
    {
        private readonly FakeModule _fakeModule;

        public ModuleDescriptorTest()
        {
            _fakeModule = new FakeModule(new FakeModuleContext());
        }

        [Fact]
        public void GetDescriptors_Returns_All_Service_Descriptors()
        {
            int numberOfExpectedDescriptors = 1;

            List<ServiceDescriptor> descriptors = _fakeModule.GetDescriptors();

            Assert.Equal(numberOfExpectedDescriptors, descriptors.Count);
        }

        [Fact]
        public void Describe_Adds_All_Descriptors_To_Given_ServiceCollection()
        {
            int numberOfExpectedAddedDescriptors = _fakeModule.GetDescriptors().Count;
            var serviceCollection = new ServiceCollection();

            _fakeModule.Describe(serviceCollection);

            Assert.Equal(numberOfExpectedAddedDescriptors, serviceCollection.Count);
        }
    }
}
