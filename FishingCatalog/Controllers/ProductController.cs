using FishingCatalog.Core;
using FishingCatalog.msCatalog.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepos;
        public ProductController(ProductRepository productRepository) {
            _productRepos = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponse>>> GetProducts()
        {
            var dbResp =  await _productRepos.GetAll();
            var response = dbResp.Select(b => new ProductResponse(b.Id, b.Name, b.Price, b.Category, b.Description, b.Image));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddProduct([FromBody] ProductRequest product)
        {
            var newProduct = new Product(
                Guid.NewGuid(),
                product.Name,
                product.Price,
                product.Category,
                product.Description,
                product.Image);
            await _productRepos.Add(newProduct);
            return Ok(newProduct.Id);
        }
    }
}
