using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using Xunit;

namespace KybInfrastructure.Server.Test
{
    public class ServiceHelperBuilderTest
    {
        [Fact]
        public void UseServiceHelper_Adds_Middleware_That_Is_Builds_ServiceHelper_To_Middleware_Pipeline()
        {
            IApplicationBuilder applicationBuilder = CreateApplicationBuilder();
            var app = applicationBuilder.Build();

            app.Invoke(CreateHttpContext());

            Assert.NotNull(ServiceLocator.Current);
        }

        private static IApplicationBuilder CreateApplicationBuilder()
        {
            IApplicationBuilder applicationBuilder = new ApplicationBuilder(It.IsAny<IServiceProvider>());
            applicationBuilder.UseServiceHelper();

            return applicationBuilder;
        }

        private static HttpContext CreateHttpContext()
        {
            Mock<HttpContext> mockCtx = new Mock<HttpContext>();
            mockCtx.Setup(ctx => ctx.RequestServices)
                .Returns(new Mock<IServiceProvider>().Object);

            return mockCtx.Object;
        }

        [Fact]
        public void UseServiceHelper_Calls_Next_Middleware_After_Adding_ServiceHelperBuilder()
        {
            IApplicationBuilder applicationBuilder = CreateApplicationBuilder();
            bool nextCalled = false;
            applicationBuilder.Use(async (context, next) =>
            {
                nextCalled = true;

                await next();
            });
            var app = applicationBuilder.Build();
            
            app.Invoke(CreateHttpContext());

            Assert.True(nextCalled);
        }
    }
}
