using KybInfrastructure.Demo.Data;
using KybInfrastructure.Server;
using Microsoft.Extensions.Logging;

namespace KybInfrastructure.Demo.Business
{
    public class BaseService
    {
        public IUnitOfWork UnitOfWork
        {
            get
            {
                var logger = ServiceLocator.Instance.GetService<ILogger<BaseService>>();
                logger.LogWarning($"Service.Current instance: {ServiceLocator.Instance.GetHashCode()}");
                var unitofwork = ServiceLocator.Instance.GetService<IUnitOfWork>();
                logger.LogWarning($"Current IUnitOfWork instance: {unitofwork.GetHashCode()}");
                return unitofwork;
            }
        }

        protected BaseService() { }
    }
}
