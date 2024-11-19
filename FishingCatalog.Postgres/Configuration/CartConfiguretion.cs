
using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.Postgres.Configuration
{
    public class CartConfiguretion : IEntityTypeConfiguration<Cart>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId);
        }
    }
}
