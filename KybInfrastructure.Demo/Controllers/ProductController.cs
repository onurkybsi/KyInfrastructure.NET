using KybInfrastructure.Demo.Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public List<Product> GetAllProducts()
            => _productService.GetAllProducts();
    }
}
