using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace KybInfrastructure.Core.Test
{
    public class ModuleDescriptorTest
    {
        private readonly List<ServiceDescriptor> serviceDescriptors;
        private readonly Mock<IModuleContext> moduleContext;

        private readonly IModuleDescriptor moduleDescriptor;

        public ModuleDescriptorTest()
        {
            serviceDescriptors = new List<ServiceDescriptor>
            {
                ServiceDescriptor.Singleton<IModuleDescriptor, ModuleDescriptorBase<IModuleContext>>(provider =>
                        new ModuleDescriptorBase<IModuleContext>(new List<ServiceDescriptor>())
                    )
            };
            moduleContext = new Mock<IModuleContext>();

            moduleDescriptor = new ModuleDescriptorBase<IModuleContext>(serviceDescriptors, moduleContext.Object);
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ServiceDescriptors_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleDescriptorBase<IModuleContext>(null));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_One_Of_ServiceDescriptor_Of_Constructor_ServiceDescriptors_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleDescriptorBase<IModuleContext>(new List<ServiceDescriptor> { null }));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ModuleContext_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleDescriptorBase<IModuleContext>(new List<ServiceDescriptor>(), null));
        }

        [Fact]
        public void GetDescriptors_Returns_All_Service_Descriptors()
        {
            int numberOfExpectedDescriptors = 1;

            List<ServiceDescriptor> descriptors = moduleDescriptor.GetDescriptors();

            Assert.Equal(numberOfExpectedDescriptors, descriptors.Count);
        }

        [Fact]
        public void Describe_Adds_All_Descriptors_To_Given_ServiceCollection()
        {
            int numberOfExpectedAddedDescriptors = moduleDescriptor.GetDescriptors().Count;
            var serviceCollection = new ServiceCollection();

            moduleDescriptor.Describe(serviceCollection);

            Assert.Equal(numberOfExpectedAddedDescriptors, serviceCollection.Count);
        }
    }
}
