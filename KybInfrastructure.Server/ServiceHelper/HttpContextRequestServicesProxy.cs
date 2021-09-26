using Microsoft.AspNetCore.Http;

namespace KybInfrastructure.Server
{
    internal class HttpContextRequestServicesProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRequestServicesProxy(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        public T GetService<T>()
            => (T)_httpContextAccessor
                    .HttpContext.RequestServices.GetService(typeof(T));
    }
}