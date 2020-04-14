using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Chats
{
    public class ChatEntityTypeConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("chat");
            builder.Property(x => x.Title).HasMaxLength(256);
            builder.Property(x => x.UserName).HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(4000);
            builder.HasIndex(x => x.TGChatId).IsUnique();
        }
    }
}
