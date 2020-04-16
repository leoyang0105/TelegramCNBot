using CNBot.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Users
{
    public class UserCommandEntityTypeConfiguration : IEntityTypeConfiguration<UserCommand>
    {
        public void Configure(EntityTypeBuilder<UserCommand> builder)
        {
            builder.ToTable("user_command");
            builder.HasOne(x => x.User).WithMany().HasForeignKey(s => s.UserId);
            builder.Property(x => x.Text).HasMaxLength(4096).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
        }
    }
}
