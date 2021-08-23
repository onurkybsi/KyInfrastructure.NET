namespace KybInfrastructure.Demo.Data
{
    public interface IUnitOfWork : KybInfrastructure.Data.IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
