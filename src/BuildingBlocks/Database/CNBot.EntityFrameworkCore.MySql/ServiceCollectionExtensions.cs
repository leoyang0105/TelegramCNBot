using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CNBot.EntityFrameworkCore.MySql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMySqlDbContext<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext, IDbContext
        {
            services.AddDbContext<IDbContext, TContext>(options =>
            {
                var migrationsAssembly = typeof(TContext).GetTypeInfo().Assembly.GetName().Name;
                options.SetupMysql(connectionString, migrationsAssembly);
            }, ServiceLifetime.Scoped);

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            return services;
        }
    }
}
