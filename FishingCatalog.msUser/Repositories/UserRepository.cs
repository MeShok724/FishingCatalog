using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msUser.Repositories
{
    public class UserRepository(FishingCatalogDbContext dbContext)
    {
        private readonly FishingCatalogDbContext _context = dbContext;

        public async Task<List<User>> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<User>> GetByRole(string name)
        {
            var dbResponse = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == name);
            if (dbResponse == null)
            {
                return [];
            }
            return dbResponse.Users;
        }
        public async Task<List<User>> GetActive()
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.IsActive == true)
                .ToListAsync();
        }

        public async Task<List<User>> SortByName(bool ask)
        {
            var resp = _context.Users
                .AsNoTracking();
            if (ask)
                resp = resp.OrderBy(p => p.Name);
            else
                resp = resp.OrderByDescending(p => p.Name);

            return await resp.ToListAsync();
        }
        public async Task<List<User>> SortByRegistrationTime(bool ask)
        {
            var resp = _context.Users
                .AsNoTracking();
            if (ask)
                resp = resp.OrderBy(p => p.CreatedAt);
            else
                resp = resp.OrderByDescending(p => p.CreatedAt);

            return await resp.ToListAsync();
        }
        public async Task<List<User>> SortByLastLoginTime(bool ask)
        {
            var resp = _context.Users
                .AsNoTracking();
            if (ask)
                resp = resp.OrderBy(p => p.LastLogin);
            else
                resp = resp.OrderByDescending(p => p.LastLogin);

            return await resp.ToListAsync();
        }
        public async Task<User?> GetById(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> Add(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                _context.SaveChanges();
                return user.Id;
            }
            catch
            {
                throw new Exception("Can not adding user in DB");
            }
        }

        public async Task<Guid> Update(User user, Guid Id)
        {
            await _context.Users
                .Where(u => u.Id == Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Name, user.Name)
                .SetProperty(u => u.Email, user.Email)
                .SetProperty(u => u.PasswordHash, user.PasswordHash)
                .SetProperty(u => u.CreatedAt, user.CreatedAt)
                .SetProperty(u => u.LastLogin, user.LastLogin)
                .SetProperty(u => u.IsActive, user.IsActive)
                .SetProperty(u => u.RoleId, user.RoleId)
                );
            return user.Id;
        }
        public async Task<Guid> ChangeIsActive(Guid id, bool isActive)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.IsActive, isActive));
            return id;
        }
        public async Task<Guid> ChangeLastLogin(Guid id, DateTime lastLogin)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.LastLogin, lastLogin));
            return id;
        }
        public async Task<Guid> Delete(Guid Id)
        {
            await _context.Users
                .Where(p => p.Id == Id)
                .ExecuteDeleteAsync();
            return Id;
        }
    }
}
