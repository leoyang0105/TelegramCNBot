using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CNBot.DistributedCache
{
    public class MemoryCacheManager : ICacheManager
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _keys = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task ClearAsync()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();

            _cancellationToken = new CancellationTokenSource();
            await Task.FromResult(0);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T result);
            return await Task.FromResult(result);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await this.GetOrSetAsync(key, acquire, CachingDefaults.CacheTime);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire, int cacheTime)
        {
            if (cacheTime <= 0 || string.IsNullOrEmpty(key))
            {
                return await acquire();
            }

            return await _memoryCache.GetOrCreateAsync(key, async entry =>
             {
                 SetupEntry(entry, key, cacheTime);
                 return await acquire();
             });
        }

        public async Task<bool> IsSetAsync(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await Task.FromResult(0);
        }

        public async Task RemoveByPrefix(string prefix)
        {
            _keys.TryRemove(prefix, out var tokenSource);
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            await Task.FromResult(0);
        }

        public async Task SetAsync<T>(string key, T data)
        {
            await this.SetAsync(key, data, CachingDefaults.CacheTime);
        }

        public async Task SetAsync<T>(string key, T data, int cacheTime)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (cacheTime <= 0)
                return;

            if (data == null)
                return;
            var entry = new MemoryCacheEntryOptions();
            SetupEntry(entry, key, cacheTime);
            await Task.FromResult(_memoryCache.Set(key, data, TimeSpan.FromSeconds(cacheTime)));
        }

        private void SetupEntry(ICacheEntry entry, string key, int cacheTime)
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTime);
            entry.AddExpirationToken(new CancellationChangeToken(_cancellationToken.Token));

            var tokenSource = _keys.GetOrAdd(key, new CancellationTokenSource());
            entry.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
        }

        private void SetupEntry(MemoryCacheEntryOptions entry, string key, int cacheTime)
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime);
            entry.AddExpirationToken(new CancellationChangeToken(_cancellationToken.Token));

            var tokenSource = _keys.GetOrAdd(key, new CancellationTokenSource());
            entry.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
        }
    }
}
