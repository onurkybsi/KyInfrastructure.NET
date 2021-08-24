using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        public UnitOfWork(MongoContext mongoContext, KybInfrastructureDemoDbContext efContext)
            : base(mongoContext, efContext) { }

        private IProductRepository productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository is null)
                    productRepository = new ProductRepository(GetContext<KybInfrastructureDemoDbContext>());
                return productRepository;
            }
        }

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository is null)
                    userRepository = new UserRepository(GetContext<MongoContext>());
                return userRepository;
            }
        }

        public override int SaveChanges()
        {
            IDatabaseContext[] contexts = GetContextsThatHaveChanges();
            int totalChanges = 0;
            foreach (var context in contexts)
            {
                totalChanges += context.SaveChanges();
            }
            return totalChanges;
        }
    }
}