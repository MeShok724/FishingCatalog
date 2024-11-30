using Microsoft.EntityFrameworkCore;
using FishingCatalog.Core;
using FishingCatalog.Postgres.Configuration;

namespace FishingCatalog.Postgres
{
    public class FishingCatalogDbContext(DbContextOptions<FishingCatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguretion());
            modelBuilder.ApplyConfiguration(new RoleConfiguretion());
            modelBuilder.ApplyConfiguration(new ProductConfiguretion());
            modelBuilder.ApplyConfiguration(new CartConfiguretion());
            modelBuilder.ApplyConfiguration(new FeedbackConfigurating());
            base.OnModelCreating(modelBuilder);
        }
    }
}
