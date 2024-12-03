using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FishinfCatalog.msAuthorization.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthenticationController(IUserRepository userRepository, IJwtProvider jwtProvider) : Controller
    {
        private readonly IUserRepository _userRepos = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

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
            HttpContext.Response.Cookies.Append("token", token);
            return Ok(token);
        }
    }
}
