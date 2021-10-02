using Microsoft.AspNetCore.Builder;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains ServiceLocator initialization strategies
    /// </summary>
    public static class ServiceLocatorInitializer
    {
        /// <summary>
        /// Initializes the ServiceLocator
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceLocator(this IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                ServiceLocator.Init(ctx);
                await next();
            });
            return app;
        }
    }
}