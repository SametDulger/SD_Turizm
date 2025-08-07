using System.Net;
using System.Text.Json;

namespace SD_Turizm.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                Success = false,
                Message = GetUserFriendlyMessage(exception),
                ErrorCode = GetErrorCode(exception),
                Timestamp = DateTime.UtcNow
            };

            context.Response.StatusCode = GetStatusCode(exception);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => "Yetkisiz erişim. Lütfen giriş yapın.",
                InvalidOperationException => "Geçersiz işlem. Lütfen verilerinizi kontrol edin.",
                ArgumentException => "Geçersiz parametre. Lütfen girdiğiniz bilgileri kontrol edin.",
                KeyNotFoundException => "Aradığınız kayıt bulunamadı.",
                _ => "Bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            };
        }

        private static string GetErrorCode(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => "UNAUTHORIZED",
                InvalidOperationException => "INVALID_OPERATION",
                ArgumentException => "INVALID_ARGUMENT",
                KeyNotFoundException => "NOT_FOUND",
                _ => "INTERNAL_ERROR"
            };
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 