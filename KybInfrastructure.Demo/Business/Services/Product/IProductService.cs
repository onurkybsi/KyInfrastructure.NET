using System.Collections.Generic;

namespace KybInfrastructure.Demo.Business
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
    }
}
