using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.EntityFrameworkCore.MySql
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
