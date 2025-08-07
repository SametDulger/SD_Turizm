using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace SD_Turizm.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, DateTime> _expirationTimes = new();

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            return await Task.FromResult(_memoryCache.Get<T>(key));
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration;
                _expirationTimes[key] = DateTime.UtcNow.Add(expiration.Value);
            }

            _memoryCache.Set(key, value, options);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            _expirationTimes.Remove(key);
            await Task.CompletedTask;
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var regex = new Regex(pattern);
            var keysToRemove = _expirationTimes.Keys.Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _expirationTimes.Remove(key);
            }

            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T? cachedValue) && cachedValue != null)
            {
                return cachedValue;
            }

            var value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }
    }
} 