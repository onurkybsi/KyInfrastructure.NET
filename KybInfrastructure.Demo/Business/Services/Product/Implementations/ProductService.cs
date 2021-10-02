using KybInfrastructure.Demo.Utils;
using System.Collections.Generic;
using System.Linq;

namespace KybInfrastructure.Demo.Business
{
    public class ProductService : BaseService, IProductService
    {
        public List<Product> GetAllProducts()
        {
            List<Data.Product> allProductsFromDb = UnitOfWork.ProductRepository
                .GetList(product => true).ToList();
            return allProductsFromDb.MapTo<List<Product>>();
        }
    }
}