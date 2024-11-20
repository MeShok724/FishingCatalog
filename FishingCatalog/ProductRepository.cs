using FishingCatalog.Core;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.msCatalog
{
    public class ProductRepository
    {
        private readonly FishingCatalogDbContext _context;
        public ProductRepository(FishingCatalogDbContext dbContext) {
            _context = dbContext;
        }

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
        public async Task<Product?> GetById(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product.Id;
        }
    }
}
