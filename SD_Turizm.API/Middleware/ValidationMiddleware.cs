using Microsoft.AspNetCore.Http;
using FluentValidation;
using SD_Turizm.Core.Exceptions;
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
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Validation failed",
                    details = ex.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }

    public static class ValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationMiddleware>();
        }
    }
} 