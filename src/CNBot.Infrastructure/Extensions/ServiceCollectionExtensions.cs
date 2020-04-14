using CNBot.Core;
using CNBot.Core.Caching;
using CNBot.Core.EventBus;
using CNBot.Core.EventBus.Abstractions;
using CNBot.Infrastructure.RabbitMQ;
using CNBot.Infrastructure.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CNBot.Infrastructure.Extensions
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
        public static IServiceCollection RegisterRedisCache(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(sp =>
            {
                var configuration = ConfigurationOptions.Parse(connectionString, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton<ICacheManager, RedisCacheManager>();
            return services;
        }
        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(RabbitMQConfiguration)).Get<RabbitMQConfiguration>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = config.HostName,
                    DispatchConsumersAsync = true
                };
                if (!string.IsNullOrEmpty(config.VirtualHost))
                {
                    factory.VirtualHost = config.VirtualHost;
                }

                if (!string.IsNullOrEmpty(config.UserName))
                {
                    factory.UserName = config.UserName;
                }

                if (!string.IsNullOrEmpty(config.Password))
                {
                    factory.Password = config.Password;
                }
                return new DefaultRabbitMQPersistentConnection(factory, logger, config.RetryCount);
            });
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, config.QueueName, config.RetryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return services;
        }
    }
}
