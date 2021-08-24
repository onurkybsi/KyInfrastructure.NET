namespace KybInfrastructure.Demo.Data
{
    public interface IUnitOfWork : KybInfrastructure.Data.IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}
