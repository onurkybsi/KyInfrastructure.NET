using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class UnitOfWork : UnitOfWorkBase<KybInfrastructureDemoDbContext>, IUnitOfWork
    {
        public UnitOfWork(KybInfrastructureDemoDbContext efContext) : base(efContext) { }

        private IProductRepository productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository is null)
                    productRepository = new ProductRepository(DatabaseContext);
                return productRepository;
            }
        }

        public override int SaveChanges()
            => DatabaseContext.SaveChanges();
    }
}