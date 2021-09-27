using Microsoft.AspNetCore.Http;
using System;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// IServiceProviderProxy implementation that use HttpContext.RequestServices
    /// </summary>
    internal class HttpContextRequestServicesProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRequestServicesProxy(IHttpContextAccessor httpContextAccessor)
        {
            ValidateIHttpContextAccessor(httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        private static void ValidateIHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public TService GetService<TService>()
            => (TService)_httpContextAccessor
                    .HttpContext
                    .RequestServices.GetService(typeof(TService));
    }
}