using Microsoft.AspNetCore.Builder;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains ServiceHelper build strategies
    /// </summary>
    public static class ServiceHelperBuilder
    {
        /// <summary>
        /// Builds ServiceHelper directly via middleware
        /// </summary>
        /// <param name="app"></param>
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