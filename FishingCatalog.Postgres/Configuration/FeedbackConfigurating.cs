using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatalog.Postgres.Configuration
{
    public class FeedbackConfigurating : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(u => u.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId);

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId);

        }
    }
}
