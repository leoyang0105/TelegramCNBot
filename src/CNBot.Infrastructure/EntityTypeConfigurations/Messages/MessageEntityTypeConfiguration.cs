using CNBot.Core.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Messages
{
    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message");
            builder.Property(x => x.Text).HasMaxLength(4096).HasCharSet("utf8mb4").HasCollation("utf8mb4_unicode_ci");
            builder.HasMany(x => x.MessageEntities).WithOne(s => s.Message).HasForeignKey(s => s.MessageId);
        }
    }
}
