using System;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Provides access to registered services in all application scope
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        /// <summary>
        /// Instance of ServiceLocator
        /// </summary>
        public static ServiceLocator Instance
        {
            get
            {
                if (_instance is null)
                    throw new InvalidOperationException("ServiceLocator must be initialized first !");
                return _instance;
            }
        }

        /// <summary>
        /// Initializes the ServiceLocator
        /// </summary>
        /// <param name="context">Current HttpContext</param>
        public static void Init(IServiceProviderProxy proxy)
        {
            ValidateIServiceProviderProxy(proxy);
            _instance = new ServiceLocator(proxy);
        }

        private static void ValidateIServiceProviderProxy(IServiceProviderProxy proxy)
        {
            if (proxy is null)
                throw new ArgumentNullException(nameof(IServiceProviderProxy));
        }

        private readonly IServiceProviderProxy _proxy;

        private ServiceLocator(IServiceProviderProxy proxy)
            => _proxy = proxy;

        /// <summary>
        /// Returns registered service from built IServiceProvider in the application
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <returns></returns>
        public TService GetService<TService>()
            where TService : class
            => _proxy.GetService<TService>();
    }
}
