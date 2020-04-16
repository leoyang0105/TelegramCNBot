using CNBot.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Users
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.Property(x => x.UserName).HasMaxLength(128);
            builder.Property(x => x.FirstName).HasMaxLength(256).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
            builder.Property(x => x.LastName).HasMaxLength(256).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
            builder.Property(x => x.LanguageCode).HasMaxLength(4000);
            builder.HasIndex(x => x.TGUserId).IsUnique();
        }
    }
}
