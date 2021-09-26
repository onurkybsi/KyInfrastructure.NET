namespace KybInfrastructure.Server
{
    public interface IServiceProviderProxy
    {
        T GetService<T>();
    }
}
