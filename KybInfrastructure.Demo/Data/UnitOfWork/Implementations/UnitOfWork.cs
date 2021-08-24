using KybInfrastructure.Data;
using System;

namespace KybInfrastructure.Demo.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoContext _mongoContext;
        private readonly KybInfrastructureDemoDbContext _efContext;

        public UnitOfWork(KybInfrastructureDemoDbContext efContext, MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _efContext = efContext;
        }

        private IProductRepository productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository is null)
                    productRepository = new ProductRepository(_efContext);
                return productRepository;
            }
        }

        private IUserRepository userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository is null)
                    userRepository = new UserRepository(_mongoContext);
                return userRepository;
            }
        }

        public int SaveChanges()
        {
            int changedInMongo = _mongoContext.SaveChanges();
            int changedInEf = _efContext.SaveChanges();
            return changedInMongo + changedInEf;
        }

        public void Dispose()
        {
            _mongoContext.Dispose();
            _efContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}