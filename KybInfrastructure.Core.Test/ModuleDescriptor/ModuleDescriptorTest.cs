using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace KybInfrastructure.Core.Test
{
    public class ModuleDescriptorTest
    {
        private class FakeModuleDescriptor : ModuleDescriptorBase<IModuleContext>
        {
            public FakeModuleDescriptor(List<ServiceDescriptor> serviceDescriptors)
                : base(serviceDescriptors) { }
            public FakeModuleDescriptor(List<ServiceDescriptor> serviceDescriptors, IModuleContext context)
                : base(serviceDescriptors, context) { }
            public IModuleContext GetContextFromBase() => base.GetContext();
        }
        private static readonly ServiceDescriptor _fakeServiceDescriptor = ServiceDescriptor.Singleton(typeof(IServiceProvider), typeof(ServiceProvider));

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ServiceDescriptors_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeModuleDescriptor(null));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_InvalidArgumentException_If_One_Of_ServiceDescriptor_Of_Constructor_ServiceDescriptors_Argument_Is_Null()
        {
            Assert.Throws<InvalidArgumentException>(() => new FakeModuleDescriptor(new List<ServiceDescriptor> { null }));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ModuleContext_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeModuleDescriptor(new List<ServiceDescriptor>(), null));
        }

        [Fact]
        public void GetDescriptors_Returns_All_Service_Descriptors()
        {
            int numberOfExpectedDescriptors = 1;

            List<ServiceDescriptor> descriptors = new FakeModuleDescriptor(new List<ServiceDescriptor> { _fakeServiceDescriptor }).GetDescriptors();

            Assert.Equal(numberOfExpectedDescriptors, descriptors.Count);
        }

        [Fact]
        public void Describe_Adds_All_Descriptors_To_Given_ServiceCollection()
        {
            IModuleDescriptor moduleDescriptor = new FakeModuleDescriptor(new List<ServiceDescriptor> { _fakeServiceDescriptor });
            int numberOfExpectedAddedDescriptors = moduleDescriptor.GetDescriptors().Count;
            var serviceCollection = new ServiceCollection();

            moduleDescriptor.Describe(serviceCollection);

            Assert.Equal(numberOfExpectedAddedDescriptors, serviceCollection.Count);
        }
        
        [Fact]
        public void GetContext_Throws_InvalidOperationException_If_Constructor_ModuleContext_Argument_Is_Null()
        {
            FakeModuleDescriptor fakeModuleDescriptor = new FakeModuleDescriptor(new List<ServiceDescriptor>());

            Assert.Throws<InvalidOperationException>(() => fakeModuleDescriptor.GetContextFromBase());
        }
    }
}
