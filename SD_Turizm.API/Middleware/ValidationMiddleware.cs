using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SD_Turizm.API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                context.Request.EnableBuffering();
                
                var originalBodyStream = context.Response.Body;
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);

                    memoryStream.Position = 0;
                    await memoryStream.CopyToAsync(originalBodyStream);
                }
                finally
                {
                    context.Response.Body = originalBodyStream;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

    public static class ValidationExtensions
    {
        public static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationMiddleware>();
        }
    }
} 