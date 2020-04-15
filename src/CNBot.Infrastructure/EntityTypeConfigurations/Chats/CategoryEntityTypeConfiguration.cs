using CNBot.Core.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Chats
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(32);
            builder.Property(c => c.Description).IsRequired(false).HasMaxLength(128);
        }
    }
}
