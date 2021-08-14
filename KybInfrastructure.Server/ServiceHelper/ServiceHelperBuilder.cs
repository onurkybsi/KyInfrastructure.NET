using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains ServiceHelper build strategies
    /// </summary>
    public static class ServiceHelperBuilder
    {
        /// <summary>
        /// Builds ServiceHelper via middleware
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="serviceHelperBuildAction">Executed build action in every scope</param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceHelper(this IApplicationBuilder app, Action<HttpContext> serviceHelperBuildAction)
        {
            app.Use(async (context, next) =>
            {
                serviceHelperBuildAction?.Invoke(context);

                await next();
            });

            return app;
        }

        /// <summary>
        /// Builds ServiceHelper directly via middleware
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceHelper(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                ServiceHelper.Build(context);

                await next();
            });

            return app;
        }
    }
}