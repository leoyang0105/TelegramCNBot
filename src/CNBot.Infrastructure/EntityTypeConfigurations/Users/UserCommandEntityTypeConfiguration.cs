using CNBot.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Infrastructure.EntityTypeConfigurations.Users
{
    public class UserCommandEntityTypeConfiguration : IEntityTypeConfiguration<UserCommand>
    {
        public void Configure(EntityTypeBuilder<UserCommand> builder)
        {
            builder.ToTable("user_command");
            builder.HasOne(x => x.User).WithMany().HasForeignKey(s => s.UserId);
        }
    }
}
