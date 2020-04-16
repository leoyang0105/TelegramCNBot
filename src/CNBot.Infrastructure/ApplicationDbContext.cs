using CNBot.Core;
using CNBot.Infrastructure.EntityTypeConfigurations.Chats;
using CNBot.Infrastructure.EntityTypeConfigurations.Messages;
using CNBot.Infrastructure.EntityTypeConfigurations.Users;
using Microsoft.EntityFrameworkCore;

namespace CNBot.Infrastructure
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChatEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChatCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserCommandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }
    }
}
