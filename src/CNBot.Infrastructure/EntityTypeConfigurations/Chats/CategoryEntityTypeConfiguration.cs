using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Chats
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(32).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
            builder.Property(c => c.Description).IsRequired(false).HasMaxLength(128).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
        }
    }
}
