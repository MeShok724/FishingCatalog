using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msCart.Repositories
{

    public class CartRepository(FishingCatalogDbContext dbContext) :ICartRepository
    {

        private readonly FishingCatalogDbContext _context = dbContext;

        public async Task<List<Cart>> GetAll()
        {
            return await _context.Carts
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Cart?> GetById(Guid id)
        {
            var dbResponse = await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            return dbResponse;
        }
        public async Task<List<Cart>> GetByUserId(Guid userId)
        {
            var dbResponse = await _context.Carts
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();
            if (dbResponse == null)
            {
                return [];
            }
            return dbResponse;
        }
        public async Task<Guid> Add(Cart cart)
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == cart.UserId);
            Product? product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == cart.ProductId);
            if (product == null || user == null)
            {
                return Guid.Empty;
            }

            await _context.Carts.AddAsync(cart);
            _context.SaveChanges();
            return cart.Id;
        }
        public async Task<Guid> Update(Cart cart, Guid id)
        {
            await _context.Carts
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.Quantity, cart.Quantity)
                .SetProperty(c => c.ModifiedAt, cart.ModifiedAt)
                );
            return cart.Id;
        }
        public async Task<Guid> Delete(Guid Id)
        {
            await _context.Carts
                .Where(c => c.Id == Id)
                .ExecuteDeleteAsync();
            return Id;
        }
        public async Task<Guid> DeleteAllByUserId(Guid userId)
        {
            await _context.Carts
                .Where(c => c.UserId == userId)
                .ExecuteDeleteAsync();
            return userId;
        }
    }
}
