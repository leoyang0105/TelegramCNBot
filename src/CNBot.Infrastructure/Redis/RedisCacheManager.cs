using CNBot.Core;
using CNBot.Core.Caching;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CNBot.Infrastructure.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        public RedisCacheManager(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }

        public async Task ClearAsync()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var keys = this.GetKeys(endPoint);
                if (keys != null && keys.Any())
                    await _database.KeyDeleteAsync(keys.ToArray());
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            var serializedItem = await _database.StringGetAsync(key);
            if (!serializedItem.HasValue)
                return default;

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default;
            return item;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await this.GetOrSetAsync(key, acquire, ApplicationDefaults.CacheTime);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire, int cacheTime)
        {
            T result = default;
            if (string.IsNullOrEmpty(key))
            {
                return result;
            }
            if (await IsSetAsync(key))
            {
                result = await this.GetAsync<T>(key);
            }
            if (result == null)
            {
                result = await acquire();
                if (result != null)
                {
                    await this.SetAsync(key, result, cacheTime);
                }
            }
            return result;
        }

        public async Task<bool> IsSetAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            if (key.Equals(ApplicationDefaults.DataProtectionKey, StringComparison.OrdinalIgnoreCase))
                return;
            await _database.KeyDeleteAsync(key);
        }

        public async Task RemoveByPrefix(string prefix)
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var keys = this.GetKeys(endPoint, prefix);
                if (keys != null && keys.Any())
                    await _database.KeyDeleteAsync(keys.ToArray());
            }
        }

        public Task SetAsync<T>(string key, T data)
        {
            return this.SetAsync(key, data, ApplicationDefaults.CacheTime);
        }

        public async Task SetAsync<T>(string key, T data, int cacheTime)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (cacheTime <= 0)
                return;

            if (data == null)
                return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            await _database.StringSetAsync(key, serializedItem, expiresIn);
        }

        private IEnumerable<RedisKey> GetKeys(EndPoint endPoint, string prefix = null)
        {
            var server = _redis.GetServer(endPoint);
            var keys = server.Keys(_database.Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*");
            keys = keys.Where(key => !key.ToString().StartsWith(ApplicationDefaults.DataProtectionKey, StringComparison.OrdinalIgnoreCase));
            return keys;
        }
    }
}
