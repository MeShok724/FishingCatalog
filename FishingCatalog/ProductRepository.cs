using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msCatalog
{
    public class ProductRepository(FishingCatalogDbContext dbContext)
    {
        private readonly FishingCatalogDbContext _context = dbContext;

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Product>> GetByCategory(string category)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<List<Product>> SortByName(bool ask)
        {
            var resp = _context.Products
                .AsNoTracking();
            if (ask)
                resp = resp.OrderBy(p => p.Name);
            else
                resp = resp.OrderByDescending(p => p.Name);

            return await resp.ToListAsync();
        }
        public async Task<List<Product>> SortByPrice(bool ask)
        {
            var resp = _context.Products
                .AsNoTracking();
            if (ask)
                resp = resp.OrderBy(p => p.Price);
            else
                resp = resp.OrderByDescending(p => p.Price);

            return await resp.ToListAsync();
        }
        public async Task<Product?> GetById(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> Add(Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return product.Id;
        }

        public async Task<Guid> Update(Product product, Guid Id)
        {
            await _context.Products
                .Where(p => p.Id == Id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(p => p.Name, product.Name)
                .SetProperty(p => p.Category, product.Category)
                .SetProperty(p => p.Price, product.Price)
                .SetProperty(p => p.Description, product.Description)
                .SetProperty(p => p.Image, product.Image)
                );
            return product.Id;
        }
        public async Task<Guid> Delete(Guid Id)
        {
            await _context.Products
                .Where(p => p.Id == Id)
                .ExecuteDeleteAsync();
            return Id;
        }
    }
}
