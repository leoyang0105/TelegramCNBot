using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace CNBot.Infrastructure.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder SetupMysql(this DbContextOptionsBuilder builder, string connectionString, string migrationsAssembly)
        {
            builder.UseMySql(connectionString,
                           mySqlOptionsAction: sqlOptions =>
                           {
                               sqlOptions.MigrationsAssembly(migrationsAssembly);
                               sqlOptions.CharSet(CharSet.Utf8);
                           });
            return builder;
        }
    }
}
