using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class ServiceHelperTest
    {
        private class FakeService { }

        private static Mock<HttpContext> CreateMockHttpContext()
        {
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(ctx => ctx.RequestServices)
                .Returns(new Mock<IServiceProvider>().Object);
            return mockHttpContext;
        }

        [Fact]
        public void Build_Throws_ArgumentNullException_If_HttpContext_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Build(null));
        }

        [Fact]
        public void Build_Throws_ArgumentNullException_If_HttpContext_RequestServices_Argument_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceLocator.Build(new Mock<HttpContext>().Object));
        }

        [Fact]
        public void Build_Initiates_ServiceHelper_Current()
        {
            Mock<HttpContext> fakeHttpContext = CreateMockHttpContext();

            ServiceLocator.Build(fakeHttpContext.Object);

            Assert.NotNull(ServiceLocator.Current);
        }

        [Fact]
        public void Build_Invokes_BeforeBuildAction_If_It_Is_Setted()
        {
            Mock<HttpContext> fakeHttpContext = CreateMockHttpContext();
            bool beforeBuildActionInvoked = false;
            Action beforeBuildAction = () => { if (!beforeBuildActionInvoked) beforeBuildActionInvoked = true; };
            ServiceLocator.SetBeforeBuildAction(beforeBuildAction);

            ServiceLocator.Build(fakeHttpContext.Object);

            Assert.True(beforeBuildActionInvoked);
        }

        [Fact]
        public void Build_Doesnt_Throw_NullReferenceException_If_BeforeBuildAction_Is_Not_Setted()
        {
            Mock<HttpContext> fakeHttpContext = CreateMockHttpContext();

            var exception = Record.Exception(() => ServiceLocator.Build(fakeHttpContext.Object));

            Assert.Null(exception);
        }

        [Fact]
        public void Build_Invokes_AfterBuildAction_If_It_Is_Setted()
        {
            Mock<HttpContext> fakeHttpContext = CreateMockHttpContext();
            bool afterBuildActionInvoked = false;
            Action afterBuildAction = () => { if (!afterBuildActionInvoked) afterBuildActionInvoked = true; };
            ServiceLocator.SetAfterBuildAction(afterBuildAction);

            ServiceLocator.Build(fakeHttpContext.Object);

            Assert.True(afterBuildActionInvoked);
        }

        [Fact]
        public void Build_Doesnt_Throw_NullReferenceException_If_AfterBuildAction_Is_Not_Setted()
        {
            Mock<HttpContext> fakeHttpContext = CreateMockHttpContext();

            var exception = Record.Exception(() => ServiceLocator.Build(fakeHttpContext.Object));

            Assert.Null(exception);
        }

        [Fact]
        public void SetBeforeBuildAction_Throws_ArgumentNullException_If_Action_Argument_Is_Null()
        {
            Mock<HttpContext> mockContext = CreateMockHttpContext();
            ServiceLocator.Build(mockContext.Object);

            Assert.Throws<ArgumentNullException>(() => ServiceLocator.SetBeforeBuildAction(null));
        }

        [Fact]
        public void SetAfterBuildAction_Throws_ArgumentNullException_If_Action_Argument_Is_Null()
        {
            Mock<HttpContext> mockContext = CreateMockHttpContext();
            ServiceLocator.Build(mockContext.Object);

            Assert.Throws<ArgumentNullException>(() => ServiceLocator.SetAfterBuildAction(null));
        }

        [Fact]
        public void GetService_Returns_Service_From_HttpContext_RequestServices()
        {
            FakeService fakeService = new FakeService();
            IServiceProvider serviceProvider = (new ServiceCollection().AddSingleton(typeof(FakeService), (serviceProvider) => fakeService))
                .BuildServiceProvider();
            Mock<HttpContext> fakeHttpContext = new Mock<HttpContext>();
            fakeHttpContext.Setup(ctx => ctx.RequestServices)
                .Returns(serviceProvider);

            ServiceLocator.Build(fakeHttpContext.Object);

            Assert.Equal(fakeService, ServiceLocator.Current.GetService<FakeService>());
        }
    }
}
