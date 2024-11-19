

using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore;

namespace FishingCatalog.Postgres.Configuration
{
    public class UserConfiguretion : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(u => u.Carts)
                .WithOne(c => c.User);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

        }
    }
}
