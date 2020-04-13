using Microsoft.Extensions.DependencyInjection;

namespace CNBot.DistributedCache
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            return services;
        }
    }
}
