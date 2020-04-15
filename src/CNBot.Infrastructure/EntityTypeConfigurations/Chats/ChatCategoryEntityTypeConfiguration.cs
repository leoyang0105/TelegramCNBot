using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Chats
{
    public class ChatCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ChatCategory>
    {
        public void Configure(EntityTypeBuilder<ChatCategory> builder)
        {
            builder.ToTable("chat_category");
            builder.HasOne(c => c.Chat).WithMany(s=>s.ChatCategories).HasForeignKey(c=>c.ChatId);
            builder.HasOne(c => c.Category).WithMany().HasForeignKey(c => c.CategoryId);

        }
    }
}
