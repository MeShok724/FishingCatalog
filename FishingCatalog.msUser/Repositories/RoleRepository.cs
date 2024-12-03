using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msUser.Repositories
{
    public class RoleRepository(FishingCatalogDbContext dbContext): IRoleRepository
    {
        private readonly FishingCatalogDbContext _context = dbContext;
        public async Task<List<Role>> GetAll()
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Guid> GetByName(string name)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name);
            return role != null ? role.Id : Guid.Empty;
        }
        public async Task<Guid> Add(Role role)
        {
            await _context.Roles.AddAsync(role);
            _context.SaveChanges();
            return role.Id;
        }
        public async Task<Guid> Delete(Guid Id)
        {
            await _context.Roles
                .Where(p => p.Id == Id)
                .ExecuteDeleteAsync();
            return Id;
        }
    }
}
