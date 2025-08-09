using System.Net;
using System.Text.Json;

namespace SD_Turizm.Web.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ErrorResponse();

            switch (exception)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Bu işlemi gerçekleştirmek için yetkiniz bulunmamaktadır.";
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Aradığınız kaynak bulunamadı.";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Geçersiz parametre gönderildi.";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case TimeoutException:
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Message = "İstek zaman aşımına uğradı. Lütfen tekrar deneyiniz.";
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    break;

                case HttpRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Message = "API servisine bağlanılamadı. Lütfen daha sonra tekrar deneyiniz.";
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = _environment.IsDevelopment() 
                        ? exception.Message 
                        : "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Development ortamında detaylı hata bilgisi ver
            if (_environment.IsDevelopment())
            {
                response.Details = new
                {
                    exception.Message,
                    exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                };
            }

            // AJAX isteği mi kontrol et
            if (IsAjaxRequest(context.Request))
            {
                var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // Normal web isteği için error sayfasına yönlendir
                context.Items["Exception"] = exception;
                context.Response.Redirect("/Home/Error");
            }
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   request.Headers["Content-Type"].ToString().Contains("application/json") ||
                   request.Path.StartsWithSegments("/api");
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public object? Details { get; set; }
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    // Extension method for easier registration
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
