using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using CNBot.EventBus.Abstractions;

namespace CNBot.EventBus.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(nameof(RabbitMQConfiguration)).Get<RabbitMQConfiguration>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = config.ConnectionString,
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
