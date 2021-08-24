using KybInfrastructure.Data;

namespace KybInfrastructure.Demo.Data
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public ProductRepository(KybInfrastructureDemoDbContext context) : base(context) { }
    }
}
