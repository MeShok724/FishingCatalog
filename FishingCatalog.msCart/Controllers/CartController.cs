using FishingCatalog.Core;
using FishingCatalog.msCart.Contracts;
using FishingCatalog.msCart.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msCart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController(ICartRepository cartRepository) : Controller
    {
        private readonly ICartRepository _cartRepository = cartRepository;

        [HttpGet]
        public async Task<ActionResult<List<Cart>>> Get()
        {
            var dbResp = await _cartRepository.GetAll();
            return Ok(dbResp);
        }
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Cart>> GetById(Guid id)
        {
            return Ok(await _cartRepository.GetById(id));
        }
        [HttpGet("byUser/{userId:Guid}")]
        public async Task<ActionResult<List<Cart>>> GetByUser(Guid userId)
        {
            return Ok( await _cartRepository.GetByUserId(userId));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] CartRequest cartRequest)
        {
            var cart = new Cart(
                Guid.NewGuid(),
                cartRequest.UserId,
                cartRequest.ProductId,
                cartRequest.Amount,
                DateTime.UtcNow,
                DateTime.UtcNow
                );
            
            Guid resp = await _cartRepository.Add(cart);
            return resp != Guid.Empty ? Ok(resp) : BadRequest(resp);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CartRequest cartRequest)
        {
            var cart = new Cart(
                id,
                cartRequest.UserId,
                cartRequest.ProductId,
                cartRequest.Amount,
                DateTime.UtcNow,
                DateTime.UtcNow
                );
            
            var dbResp = await _cartRepository.Update(cart, id);
            return Ok(dbResp);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var dbResp = await _cartRepository.Delete(id);
            return Ok(dbResp);
        }

        [HttpDelete("byUser/{userId:Guid}")]
        public async Task<ActionResult<Guid>> DeleteByUser(Guid userId)
        {
            var dbResp = await _cartRepository.DeleteAllByUserId(userId);
            return Ok(dbResp);
        }
    }
}
