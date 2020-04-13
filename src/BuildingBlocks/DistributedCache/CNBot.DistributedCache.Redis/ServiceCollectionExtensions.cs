using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CNBot.DistributedCache.Redis
{
    public static class ServiceCollectionExtensions
    {
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
    }
}
