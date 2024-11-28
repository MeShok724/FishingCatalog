using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msUser.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class RegistrationController(UserRepository userRepository) : Controller
    {
        private readonly UserRepository _userRepos = userRepository;

        [HttpPost]
        public async Task<ActionResult<Guid>> Registrate([FromBody] RegistrationRequest userRequest)
        {
            Guid roleId = await _userRepos.GetDafaultRoleId();
            if (roleId == Guid.Empty)
                return BadRequest("Default role not found");
            if (userRequest.Password.Length < 8 || userRequest.Password.Length > 16)
                return BadRequest("The password must be longer than 8 and shorter than 16 characters");
            string hash = PasswordHasher.Generate(userRequest.Password);
            var newUser = Core.User.Create(
                Guid.NewGuid(),
                userRequest.Name,
                userRequest.Email,
                hash,
                DateTime.UtcNow,
                DateTime.UtcNow,
                true,
                roleId
                );
            if (!string.IsNullOrEmpty(newUser.Item2))
            {
                Console.WriteLine(newUser.Item2);
                return BadRequest(newUser.Item2);
            }
            var reposResp = await _userRepos.Add(newUser.Item1);

            if (!string.IsNullOrEmpty(reposResp.Item2))
                return BadRequest(reposResp.Item2);
            else
                return Ok(newUser.Item1.Id);
        }
    }
}
