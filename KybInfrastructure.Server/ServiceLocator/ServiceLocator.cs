using Microsoft.AspNetCore.Http;
using System;
using System.Threading;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Provides access to registered services in all application scope
    /// </summary>
    public class ServiceLocator
    {
        private static readonly AsyncLocal<ServiceLocator> _instance = new();

        /// <summary>
        /// Instance of ServiceLocator
        /// </summary>
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance.Value is null)
                    throw new InvalidOperationException("ServiceLocator must be initialized first !");
                return _instance.Value;
            }
        }

        /// <summary>
        /// Initializes the ServiceLocator
        /// </summary>
        /// <param name="context">Current HttpContext</param>
        public static void Init(HttpContext context)
        {
            ValidateHttpContext(context);
            _instance.Value = new ServiceLocator(context);
        }

        private static void ValidateHttpContext(HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (context.RequestServices is null)
                throw new ArgumentNullException(nameof(context.RequestServices));
        }

        private readonly HttpContext _context;

        private ServiceLocator(HttpContext context)
            => _context = context;

        /// <summary>
        /// Returns registered service from built IServiceProvider in the application
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <returns></returns>
        public TService GetService<TService>()
            where TService : class
            => (TService)_context.RequestServices.GetService(typeof(TService));
    }
}