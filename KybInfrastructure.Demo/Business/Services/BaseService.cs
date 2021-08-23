using KybInfrastructure.Demo.Data;
using KybInfrastructure.Server;

namespace KybInfrastructure.Demo.Business
{
    public class BaseService
    {
        public IUnitOfWork UnitOfWork => ServiceHelper.Current.GetService<IUnitOfWork>();
        protected BaseService() { }
    }
}
