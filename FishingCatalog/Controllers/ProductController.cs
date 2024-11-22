using FishingCatalog.Core;
using FishingCatalog.msCatalog.Contracts;
using FishingCatalog.msCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msCatalog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ProductRepository productRepository) : ControllerBase
    {
        private readonly ProductRepository _productRepos = productRepository;

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
            var newProduct = Product.Create(
                Guid.NewGuid(),
                product.Name,
                product.Price,
                product.Category,
                product.Description,
                product.Image);
            if (newProduct.Item2 != null)
            {
                return BadRequest(newProduct.Item2);
            }
            await _productRepos.Add(newProduct.Item1);
            return Ok(newProduct.Item1.Id);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProductResponse>> GetById(Guid id)
        {
            var dbResp = await _productRepos.GetById(id);
            if (dbResp ==  null)
            {
                return BadRequest();
            }
            var resp = new ProductResponse(dbResp.Id, dbResp.Name, dbResp.Price, dbResp.Category, dbResp.Description, dbResp.Image);
            return Ok(resp);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] ProductRequest product)
        {
            var toUpdateProduct = Product.Create(
                id,
                product.Name,
                product.Price,
                product.Category,
                product.Description,
                product.Image);
            if (toUpdateProduct.Item2 != null)
            {
                return BadRequest(toUpdateProduct.Item2);
            }
            var dbResp = await _productRepos.Update(toUpdateProduct.Item1, id);
            return Ok(dbResp);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var dbResp = await _productRepos.Delete(id);
            return Ok(dbResp);
        }

        [HttpGet("{category}")]
        public async Task<ActionResult<List<ProductResponse>>> GetByCategory(String category)
        {
            var dbResp = await _productRepos.GetByCategory(category);
            var resp = dbResp.Select(p => new ProductResponse(
                p.Id, p.Name, p.Price, p.Category, p.Description, p.Image));
            return Ok(resp);
        }
        [HttpGet("{field}/{ask}")]
        public async Task<ActionResult<List<ProductResponse>>> Sort(string field, bool ask)
        {
            List<Product> dbResp = [];
            if (field == "price")
                dbResp = await _productRepos.SortByPrice(ask);
            else if (field == "name")
                dbResp = await _productRepos.SortByName(ask);
            var resp = dbResp.Select(p => new ProductResponse(
                p.Id, p.Name, p.Price, p.Category, p.Description, p.Image));
            return Ok(resp);
        }
    }
}
