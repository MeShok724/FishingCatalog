using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msUser.Repositories
{
    public class RoleRepository(FishingCatalogDbContext dbContext)
    {
        private readonly FishingCatalogDbContext _context = dbContext;
        public async Task<List<Role>> GetAll()
        {
            return await _context.Roles
                .AsNoTracking()
                .ToListAsync();
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
