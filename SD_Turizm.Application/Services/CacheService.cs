using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace SD_Turizm.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, DateTime> _expirationTimes = new();
        private readonly Dictionary<string, TimeSpan> _cacheDurations;
        private readonly Dictionary<string, int> _cacheHits = new();
        private readonly Dictionary<string, int> _cacheMisses = new();

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheDurations = new Dictionary<string, TimeSpan>
            {
                { "vendors", TimeSpan.FromMinutes(30) },
                { "hotels", TimeSpan.FromMinutes(15) },
                { "tours", TimeSpan.FromMinutes(20) },
                { "sales", TimeSpan.FromMinutes(10) },
                { "reports", TimeSpan.FromMinutes(5) },
                { "user", TimeSpan.FromHours(1) },
                { "statistics", TimeSpan.FromMinutes(5) }
            };
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T? value))
            {
                IncrementCacheHit(key);
                return value;
            }
            
            IncrementCacheMiss(key);
            return await Task.FromResult<T?>(default);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions();
            
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration;
                _expirationTimes[key] = DateTime.UtcNow.Add(expiration.Value);
            }
            else
            {
                // Cache type'a göre otomatik süre belirle
                var cacheType = GetCacheType(key);
                if (_cacheDurations.ContainsKey(cacheType))
                {
                    var duration = _cacheDurations[cacheType];
                    options.AbsoluteExpirationRelativeToNow = duration;
                    _expirationTimes[key] = DateTime.UtcNow.Add(duration);
                }
                else
                {
                    var defaultDuration = TimeSpan.FromMinutes(10);
                    options.AbsoluteExpirationRelativeToNow = defaultDuration;
                    _expirationTimes[key] = DateTime.UtcNow.Add(defaultDuration);
                }
            }

            // Cache eviction callback
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                Console.WriteLine($"Cache item '{key}' was evicted. Reason: {reason}");
                _expirationTimes.Remove(key?.ToString() ?? "");
            });

            _memoryCache.Set(key, value, options);
            await Task.CompletedTask;
        }

        public async Task SetWithSlidingExpirationAsync<T>(string key, T value, TimeSpan slidingExpiration)
        {
            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };

            _memoryCache.Set(key, value, options);
            _expirationTimes[key] = DateTime.UtcNow.Add(slidingExpiration);
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
                IncrementCacheHit(key);
                return cachedValue;
            }

            IncrementCacheMiss(key);
            var value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }

        public async Task ClearAllAsync()
        {
            if (_memoryCache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
            }
            _expirationTimes.Clear();
            _cacheHits.Clear();
            _cacheMisses.Clear();
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, object>> GetCacheStatsAsync()
        {
            var stats = new Dictionary<string, object>
            {
                { "TotalItems", _expirationTimes.Count },
                { "TotalHits", _cacheHits.Values.Sum() },
                { "TotalMisses", _cacheMisses.Values.Sum() },
                { "HitRate", _cacheHits.Values.Sum() + _cacheMisses.Values.Sum() > 0 
                    ? (double)_cacheHits.Values.Sum() / (_cacheHits.Values.Sum() + _cacheMisses.Values.Sum()) * 100 
                    : 0 },
                { "CacheType", "MemoryCache" },
                { "ExpiringItems", _expirationTimes.Count(x => x.Value <= DateTime.UtcNow.AddMinutes(5)) }
            };

            return await Task.FromResult(stats);
        }

        private string GetCacheType(string key)
        {
            if (key.Contains("vendor", StringComparison.OrdinalIgnoreCase))
                return "vendors";
            if (key.Contains("hotel", StringComparison.OrdinalIgnoreCase))
                return "hotels";
            if (key.Contains("tour", StringComparison.OrdinalIgnoreCase))
                return "tours";
            if (key.Contains("sale", StringComparison.OrdinalIgnoreCase))
                return "sales";
            if (key.Contains("report", StringComparison.OrdinalIgnoreCase))
                return "reports";
            if (key.Contains("user", StringComparison.OrdinalIgnoreCase))
                return "user";
            if (key.Contains("stat", StringComparison.OrdinalIgnoreCase))
                return "statistics";
            
            return "default";
        }

        private void IncrementCacheHit(string key)
        {
            _cacheHits[key] = _cacheHits.GetValueOrDefault(key, 0) + 1;
        }

        private void IncrementCacheMiss(string key)
        {
            _cacheMisses[key] = _cacheMisses.GetValueOrDefault(key, 0) + 1;
        }
    }
} 