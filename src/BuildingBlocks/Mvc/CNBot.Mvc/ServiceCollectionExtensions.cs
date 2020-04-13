using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CNBot.DistributedCache;
using CNBot.DistributedCache.Redis;
using CNBot.EntityFrameworkCore;
using CNBot.EntityFrameworkCore.MySql;
using CNBot.EventBus;
using CNBot.EventBus.RabbitMQ;

namespace CNBot.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
               where TContext : DbContext, IDbContext
        {
            var dbConnectionString = configuration.GetConnectionString(ApplicationDefaults.DbConnectionStringKey);
            var dbProviderType = configuration.GetValue<DatabaseType>(ApplicationDefaults.DbProviderTypeKey);
            switch (dbProviderType)
            {
                case DatabaseType.MySql:
                    services.RegisterMySqlDbContext<TContext>(dbConnectionString);
                    break;
                //case DatabaseType.SqlServer:
                //    services.RegisterSqlServerDbContext<TContext>(dbConnectionString);
                //    break;
                //case DatabaseType.PostgreSQL:
                //    services.RegisterNpgsqlDbContext<TContext>(dbConnectionString);
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(DatabaseType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseType)))}.");
            }
            return services;
        }
        public static IServiceCollection RegisterDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheProviderType = configuration.GetValue<CacheType>(ApplicationDefaults.CacheProviderTypeKey);
            switch (cacheProviderType)
            {
                case CacheType.MemoryCache:
                    services.RegisterMemoryCache();
                    break;
                case CacheType.Redis:
                    var redisConnectionString = configuration.GetConnectionString(ApplicationDefaults.DbConnectionStringKey);
                    if (string.IsNullOrEmpty(redisConnectionString))
                        throw new ArgumentNullException("The redis connection was not found.");
                    services.RegisterRedisCache(redisConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(CacheType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(CacheType)))}.");
            }
            return services;
        }
        public static IServiceCollection RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusType = configuration.GetValue<EventBusType>(ApplicationDefaults.CacheProviderTypeKey);
            switch (eventBusType)
            {
                case EventBusType.RabbitMQ:
                    services.RegisterRabbitMQ(configuration);
                    break;
                case EventBusType.RocketMQ:
                     // TODO Impl
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(CacheType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(EventBusType)))}.");
            }
            return services;
        }
    }
}
