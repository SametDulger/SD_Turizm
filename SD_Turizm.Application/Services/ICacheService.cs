namespace SD_Turizm.Application.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task SetWithSlidingExpirationAsync<T>(string key, T value, TimeSpan slidingExpiration);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task<bool> ExistsAsync(string key);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task ClearAllAsync();
        Task<Dictionary<string, object>> GetCacheStatsAsync();
    }
} 