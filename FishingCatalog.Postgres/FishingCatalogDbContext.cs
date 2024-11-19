using Microsoft.EntityFrameworkCore;
using FishingCatalog.Core;

namespace FishingCatalog.Postgres
{
    public class FishingCatalogDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    } 
}
