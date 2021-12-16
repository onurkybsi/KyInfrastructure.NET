using KybInfrastructure.Core.UtilityExceptions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

            public FakeModuleDescriptor(List<ServiceDescriptor> serviceDescriptors, List<Type> serviceTypesThatMustBeDescribed)
                : base(serviceDescriptors, serviceTypesThatMustBeDescribed) { }

            public FakeModuleDescriptor(List<ServiceDescriptor> serviceDescriptors, IModuleContext context, List<Type> serviceTypesThatMustBeDescribed)
                : base(serviceDescriptors, context, serviceTypesThatMustBeDescribed) { }

            public IModuleContext GetContextFromBase() => base.GetContext();
        }
        private static readonly ServiceDescriptor _fakeServiceDescriptor = ServiceDescriptor.Singleton(typeof(IServiceProvider), typeof(ServiceProvider));

        private interface IFakeService { }
        private class FakeService : IFakeService { }

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
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ServiceTypes_That_MustBeDescribed_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeModuleDescriptor(new List<ServiceDescriptor>(), (new Mock<IModuleContext>()).Object, null));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_One_Of_ServiceDescriptor_Of_Constructor_ServiceTypes_That_MustBeDescribed_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeModuleDescriptor(new List<ServiceDescriptor>(), (new Mock<IModuleContext>()).Object, new List<Type>
            {
                typeof(ServiceDescriptor),
                null
            }));
        }

        [Fact]
        public void ModuleDescriptorBase_Throws_ArgumentNullException_If_Constructor_ModuleContext_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new FakeModuleDescriptor(new List<ServiceDescriptor>(), (IModuleContext)null));
        }

        [Fact]
        public void GetDescriptors_Returns_All_Service_Descriptors()
        {
            int numberOfExpectedDescriptors = 1;

            List<ServiceDescriptor> descriptors = new FakeModuleDescriptor(new List<ServiceDescriptor> { _fakeServiceDescriptor }).GetDescriptors();

            Assert.Equal(numberOfExpectedDescriptors, descriptors.Count);
        }

        [Fact]
        public void Describe_Throws_ModuleLoadingException_If_One_Of_Dependent_Service_Was_Not_Registered_To_Given_ServiceCollection()
        {
            IModuleDescriptor moduleDescriptor = new FakeModuleDescriptor(new List<ServiceDescriptor> { }, new List<Type>
            {
                typeof(IFakeService)
            });
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ModuleLoadingException>(() => moduleDescriptor.Describe(serviceCollection));
        }

        [Fact]
        public void Describe_Does_Not_Throws_ModuleLoadingException_If_All_Dependent_Service_Was_Registered_To_Given_ServiceCollection()
        {
            IModuleDescriptor moduleDescriptor = new FakeModuleDescriptor(new List<ServiceDescriptor> { }, new List<Type>
            {
                typeof(IFakeService)
            });
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IFakeService, FakeService>();

            var exception = Record.Exception(() => moduleDescriptor.Describe(serviceCollection));

            Assert.Null(exception);
        }

        [Fact]
        public void Describe_Does_Not_Throws_ModuleLoadingException_If_Dependent_Services_Is_Not_Specified()
        {
            IModuleDescriptor moduleDescriptor = new FakeModuleDescriptor(new List<ServiceDescriptor> { });
            var serviceCollection = new ServiceCollection();

            var exception = Record.Exception(() => moduleDescriptor.Describe(serviceCollection));

            Assert.Null(exception);
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
