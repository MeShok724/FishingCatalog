using FishinfCatalog.msAuthorization;
using FishingCatalog.Core;
using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(UserRepository userRepository, RabbitMQService rabbitMQService) : Controller
    {
        private readonly UserRepository _userRepos = userRepository;
        private readonly RabbitMQService _rabbitMQService = rabbitMQService;

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var dbResp = await _userRepos.GetAll();
            var response = dbResp.Select(b => new UserResponse(b.Id, b.Name, b.Email, b.PasswordHash, b.CreatedAt, b.LastLogin, b.IsActive, b.RoleId));
            return Ok(response);
        }
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            var dbResp = await _userRepos.GetById(id);
            if (dbResp == null)
            {
                return BadRequest();
            }
            var resp = new UserResponse(dbResp.Id, dbResp.Name, dbResp.Email, dbResp.PasswordHash,
                dbResp.CreatedAt, dbResp.LastLogin, dbResp.IsActive, dbResp.RoleId);
            return Ok(resp);
        }


        [HttpGet("{role}")]
        public async Task<ActionResult<List<UserResponse>>> GetByRole(String role)
        {
            var dbResp = await _userRepos.GetByRole(role);
            var resp = dbResp.Select(p => new UserResponse(
                p.Id, p.Name, p.Email, p.PasswordHash, p.CreatedAt, p.LastLogin, p.IsActive, p.RoleId));
            return Ok(resp);
        }

        [HttpGet("{field}/{ask}")]
        public async Task<ActionResult<List<UserResponse>>> Sort(string field, bool ask)
        {
            List<User> dbResp = [];
            if (field == "name")
                dbResp = await _userRepos.SortByName(ask);
            else if (field == "registrationTime")
                dbResp = await _userRepos.SortByRegistrationTime(ask);
            else if (field == "lastLoginTime")
                dbResp = await _userRepos.SortByLastLoginTime(ask);

            var resp = dbResp.Select(p => new UserResponse(
                p.Id, p.Name, p.Email, p.PasswordHash, p.CreatedAt, p.LastLogin, p.IsActive, p.RoleId));
            return Ok(resp);
        }

        [HttpGet("getActive")]
        public async Task<ActionResult<List<UserResponse>>> GetActive()
        {
            var dbResp = await _userRepos.GetActive();
            var resp = dbResp.Select(p => new UserResponse(
                p.Id, p.Name, p.Email, p.PasswordHash, p.CreatedAt, p.LastLogin, p.IsActive, p.RoleId));
            return Ok(resp);

        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UserToUpdate product)
        {
            var toUpdateProduct = Core.User.Create(
                id,
                product.Name,
                product.Email,
                product.PasswordHash,
                product.CreatedAt,
                product.LastLogin,
                product.IsActive,
                product.RoleId
                );
            if (!string.IsNullOrEmpty(toUpdateProduct.Item2))
            {
                return BadRequest(toUpdateProduct.Item2);
            }
            Guid resp = await _userRepos.Update(toUpdateProduct.Item1, id);

            if (resp == Guid.Empty)
                return BadRequest(resp);
            else
                return Ok(resp);
        }

        [HttpPut("isActive/{id:Guid}/{isActive}")]
        public async Task<ActionResult<Guid>> UpdateIsActive(Guid id, bool isActive)
        {
            await _userRepos.ChangeIsActive(id, isActive);
            return id;
        }

        [HttpPut("lastLogin/{id:Guid}/{lastLogin}")]
        public async Task<ActionResult<Guid>> UpdateLastLogin(Guid id, DateTime lastLogin)
        {
            await _userRepos.ChangeLastLogin(id, lastLogin);
            return id;
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var dbResp = await _userRepos.Delete(id);
            await _rabbitMQService.SendGuidAsync(id);
            return Ok(dbResp);
        }
    }
}
