using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Chats
{
    public class ChatMemberEntityTypeConfiguration : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.ToTable("chat_member");
            builder.HasOne(x => x.Chat).WithMany().HasForeignKey(s => s.ChatId);
        }
    }
}
