using FishingCatalog.Core;
using FishingCatalog.msUser.Contracts;
using FishingCatalog.msUser.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(RoleRepository roleRepository) : Controller
    {
        private readonly RoleRepository _roleRepos = roleRepository;

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var dbResp = await _roleRepos.GetAll();
            return Ok(dbResp);
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<Role>> GetByName(string name)
        {
            var dbResp = await _roleRepos.GetByName(name);
            return Ok(dbResp);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] string name)
        {
            var role = Role.Create(Guid.NewGuid(), name);
            if (!string.IsNullOrEmpty(role.Item2))
            {
                return BadRequest(role.Item2);
            }
            await _roleRepos.Add(role.Item1);
            return Ok(role.Item1.Id);
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            await _roleRepos.Delete(id);
            return Ok(id);
        }
    }
}
