using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SD_Turizm.Application.Services
{
    public interface ILoggingService
    {
        void LogInformation(string message, object? data = null);
        void LogWarning(string message, object? data = null);
        void LogError(string message, Exception? exception = null, object? data = null);
        void LogDebug(string message, object? data = null);
        void LogTrace(string message, object? data = null);
        void LogCritical(string message, Exception? exception = null, object? data = null);
    }

    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, object? data = null)
        {
            var logData = CreateLogData(message, data);
            _logger.LogInformation("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
        }

        public void LogWarning(string message, object? data = null)
        {
            var logData = CreateLogData(message, data);
            _logger.LogWarning("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
        }

        public void LogError(string message, Exception? exception = null, object? data = null)
        {
            var logData = CreateLogData(message, data, exception);
            if (exception != null)
            {
                _logger.LogError(exception, "{Message} | {Data}", message, JsonSerializer.Serialize(logData));
            }
            else
            {
                _logger.LogError("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
            }
        }

        public void LogDebug(string message, object? data = null)
        {
            var logData = CreateLogData(message, data);
            _logger.LogDebug("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
        }

        public void LogTrace(string message, object? data = null)
        {
            var logData = CreateLogData(message, data);
            _logger.LogTrace("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
        }

        public void LogCritical(string message, Exception? exception = null, object? data = null)
        {
            var logData = CreateLogData(message, data, exception);
            if (exception != null)
            {
                _logger.LogCritical(exception, "{Message} | {Data}", message, JsonSerializer.Serialize(logData));
            }
            else
            {
                _logger.LogCritical("{Message} | {Data}", message, JsonSerializer.Serialize(logData));
            }
        }

        private object CreateLogData(string message, object? data = null, Exception? exception = null)
        {
            return new
            {
                Timestamp = DateTime.UtcNow,
                Message = message,
                Data = data,
                Exception = exception != null ? new
                {
                    Type = exception.GetType().Name,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                } : null,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            };
        }
    }
}
