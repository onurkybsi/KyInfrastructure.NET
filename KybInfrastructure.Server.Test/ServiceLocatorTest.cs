using Microsoft.AspNetCore.Http;
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
        public void Init_Throws_ArgumentNullException_If_Given_HttpContext_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Init(default));
        }

        [Fact, Priority(1)]
        public void Init_Throws_ArgumentNullException_If_Given_HttpContext_RequestServices_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Init(new Mock<HttpContext>().Object));
        }

        [Fact, Priority(3)]
        public void Init_Sets_Instance()
        {
            Mock<HttpContext> mockHttpContext = new();
            Mock<IServiceProvider> mockServiceProvider = new();
            mockHttpContext.SetupGet(ctx => ctx.RequestServices)
                .Returns(mockServiceProvider.Object);

            ServiceLocator.Init(mockHttpContext.Object);

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
            Mock<HttpContext> mockHttpContext = new();
            mockHttpContext.SetupGet(ctx => ctx.RequestServices)
                .Returns(new Mock<IServiceProvider>().Object);

            ServiceLocator.Init(mockHttpContext.Object);

            Assert.NotNull(ServiceLocator.Instance);
        }

        [Fact, Priority(5)]
        public void GetService_Returns_Registered_Service_Via_HttpContext_RequestServices()
        {
            Mock<HttpContext> mockHttpContext = new();
            Mock<IServiceProvider> mockServiceProvider = new();
            MockService mockService = new();
            mockServiceProvider.Setup(sp => sp.GetService(typeof(MockService)))
                .Returns(mockService);
            mockHttpContext.SetupGet(ctx => ctx.RequestServices)
                .Returns(mockServiceProvider.Object);
            ServiceLocator.Init(mockHttpContext.Object);

            MockService mockServiceFromServiceLocator = ServiceLocator.Instance.GetService<MockService>();

            Assert.Equal(mockService, mockServiceFromServiceLocator);
        }
    }
}