using CNBot.Core.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Messages
{
    public class MessageEntityEntityTypeConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("message_entity");
            builder.Property(x => x.Url).HasMaxLength(1024);
            builder.Property(x => x.Language).HasMaxLength(4096);
        }
    }
}
