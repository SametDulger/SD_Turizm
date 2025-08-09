using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace SD_Turizm.API.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisHealthCheck(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = _redis.GetDatabase();
                await database.PingAsync();
                
                return HealthCheckResult.Healthy("Redis is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Redis is unhealthy", ex);
            }
        }
    }
}
