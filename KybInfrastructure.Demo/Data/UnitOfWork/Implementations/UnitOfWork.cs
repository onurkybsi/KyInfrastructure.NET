using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UnitOfWork : UnitOfWorkBase<KybInfrastructureDemoDbContext>, IUnitOfWork
    {
        public UnitOfWork(KybInfrastructureDemoDbContext dbContext) : base(dbContext) { }

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository is null)
                    userRepository = new UserRepository(DatabaseContext);
                return userRepository;
            }
        }

        public override int SaveChanges()
            => DatabaseContext.SaveChanges();
    }
}