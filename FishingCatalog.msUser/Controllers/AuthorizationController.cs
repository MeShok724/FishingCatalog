using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishinfCatalog.msAuthorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController(UserRepository userRepository, JwtProvider jwtProvider) : Controller
    {
        private readonly UserRepository _userRepos = userRepository;
        private readonly JwtProvider _jwtProvider = jwtProvider;

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] AuthorizationRequest authorizationRequest)
        {
            var user = await _userRepos.GetByEmail(authorizationRequest.Email);
            if (user == null)
            {
                return NotFound();
            }
            if (!PasswordHasher.Verify(authorizationRequest.Password, user.PasswordHash))
                return BadRequest("Wrong password");

            var token = _jwtProvider.GenerateToken(user);
            return Ok(token);
        }
    }
}
