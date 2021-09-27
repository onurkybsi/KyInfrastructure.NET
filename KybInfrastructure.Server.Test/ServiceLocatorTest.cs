using Moq;
using System;
using Xunit;
using Xunit.Priority;

namespace KybInfrastructure.Server.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ServiceLocatorTest
    {
        public class MockService { }

        [Fact, Priority(1)]
        public void Init_Throws_ArgumentNullException_If_IServiceProviderProxy_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Init(default));
        }

        [Fact, Priority(3)]
        public void Init_Sets_Instance()
        {
            Mock<IServiceProviderProxy> mockServiceProviderProxy = new();

            ServiceLocator.Init(mockServiceProviderProxy.Object);

            Assert.NotNull(ServiceLocator.Instance);
        }

        [Fact, Priority(2)]
        public void Instance_Throws_InvalidOperationException_If_ServiceLocator_Didnt_Initialize_Before()
        {
            Assert.Throws<InvalidOperationException>(() => { var instance = ServiceLocator.Instance; });
        }

        [Fact, Priority(4)]
        public void Instance_Returns_ServiceLocator_Instance_If_Init_Executed()
        {
            Mock<IServiceProviderProxy> mockServiceProviderProxy = new();

            ServiceLocator.Init(mockServiceProviderProxy.Object);

            Assert.NotNull(ServiceLocator.Instance);
        }

        [Fact, Priority(5)]
        public void GetService_Returns_Registered_Service_Via_IServiceProviderProxy()
        {
            Mock<IServiceProviderProxy> mockServiceProviderProxy = new();
            MockService mockService = new();
            mockServiceProviderProxy.Setup(sp => sp.GetService<MockService>())
                .Returns(mockService);
            ServiceLocator.Init(mockServiceProviderProxy.Object);

            MockService mockServiceFromServiceLocator = ServiceLocator.Instance.GetService<MockService>();

            Assert.Equal(mockService, mockServiceFromServiceLocator);
        }
    }
}