namespace KybInfrastructure.Server
{
    /// <summary>
    /// Module that proxy to IServiceProvider
    /// </summary>
    public interface IServiceProviderProxy
    {
        /// <summary>
        /// Returns registered service from IServiceProvider
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService GetService<TService>();
    }
}
