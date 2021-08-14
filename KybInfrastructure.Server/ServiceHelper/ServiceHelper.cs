using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Helper that provides service access in all application scope
    /// </summary>
    public class ServiceHelper
    {
        /// <summary>
        /// Current instance of ServiceHelper
        /// </summary>
        public static ServiceHelper Current { get; private set; }

        /// <summary>
        /// Builds current ServiceHelper
        /// </summary>
        /// <param name="context">Current HttpContext</param>
        public static void Build(HttpContext context)
        {
            beforeBuildAction?.Invoke();
            Current = new ServiceHelper(context);
            afterBuildAction?.Invoke();
        }

        private static Action beforeBuildAction;
        /// <summary>
        /// Sets action which executes before ServiceHelper build
        /// </summary>
        /// <param name="action">Action</param>
        public static void SetBeforeBuildAction(Action action)
            => beforeBuildAction = action;

        private static Action afterBuildAction;
        /// <summary>
        /// Sets action which executes after ServiceHelper build
        /// </summary>
        /// <param name="action">Action</param>
        public static void SetAfterBuildAction(Action action)
            => afterBuildAction = action;


        private readonly HttpContext _httpContext;

        private ServiceHelper(HttpContext context)
        {
            _httpContext = context;
        }

        /// <summary>
        /// Returns requested service from application service collections
        /// </summary>
        /// <typeparam name="T">Type of service</typeparam>
        /// <returns></returns>
        public T GetService<T>()
            => _httpContext.RequestServices.GetRequiredService<T>();
    }
}
