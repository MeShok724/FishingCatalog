

using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.Postgres.Configuration
{
    public class ProductConfiguretion : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
