
using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.Postgres.Configuration
{
    public class RoleConfiguretion : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role);

        }
    }
}
