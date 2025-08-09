using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using System.Text.Json;
using SD_Turizm.API.Middleware;
using SD_Turizm.Core.Exceptions;
using System.Text;
using FluentValidation;
using FluentValidation.Results;

namespace SD_Turizm.Tests.Unit.Middleware
{
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
        private readonly GlobalExceptionHandler _middleware;
        private readonly DefaultHttpContext _httpContext;

        public GlobalExceptionHandlerTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            _middleware = new GlobalExceptionHandler(_nextMock.Object, _loggerMock.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        [Fact]
        public async Task InvokeAsync_WithNoException_ShouldCallNext()
        {
            // Arrange
            _nextMock.Setup(x => x(_httpContext)).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _nextMock.Verify(x => x(_httpContext), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithBusinessException_ShouldHandleCorrectly()
        {
            // Arrange
            var businessException = new BusinessException("İş kuralı hatası", "BUSINESS_ERROR", 400);
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(businessException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            response.ValueKind.Should().Be(JsonValueKind.Object);

            // Verify logging
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    businessException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithValidationException_ShouldHandleCorrectly()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Email", "Email is invalid")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("VALIDATION_ERROR");
            responseBody.Should().Contain("Name is required");
            responseBody.Should().Contain("Email is invalid");
        }

        [Fact]
        public async Task InvokeAsync_WithUnauthorizedAccessException_ShouldHandleCorrectly()
        {
            // Arrange
            var unauthorizedException = new UnauthorizedAccessException("Access denied");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(unauthorizedException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(401);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("UNAUTHORIZED");
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Yetkisiz erişim");
        }

        [Fact]
        public async Task InvokeAsync_WithInvalidOperationException_ShouldHandleCorrectly()
        {
            // Arrange
            var invalidOpException = new InvalidOperationException("Invalid operation");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(invalidOpException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("INVALID_OPERATION");
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Geçersiz işlem");
        }

        [Fact]
        public async Task InvokeAsync_WithArgumentException_ShouldHandleCorrectly()
        {
            // Arrange
            var argumentException = new ArgumentException("Invalid argument");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(argumentException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("INVALID_ARGUMENT");
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Geçersiz parametre");
        }

        [Fact]
        public async Task InvokeAsync_WithKeyNotFoundException_ShouldHandleCorrectly()
        {
            // Arrange
            var notFoundException = new KeyNotFoundException("Key not found");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(notFoundException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(404);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("NOT_FOUND");
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Aradığınız kayıt bulunamadı");
        }

        [Fact]
        public async Task InvokeAsync_WithGenericException_ShouldHandleCorrectly()
        {
            // Arrange
            var genericException = new Exception("Some unexpected error");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(genericException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(500);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("INTERNAL_ERROR");
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Bir hata oluştu");
        }

        [Fact]
        public async Task InvokeAsync_ResponseShouldHaveCorrectStructure()
        {
            // Arrange
            var exception = new ArgumentException("Test exception");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            
            response.TryGetProperty("success", out var successProperty).Should().BeTrue();
            successProperty.GetBoolean().Should().BeFalse();
            
            response.TryGetProperty("message", out var messageProperty).Should().BeTrue();
            messageProperty.GetString().Should().NotBeNullOrEmpty();
            
            response.TryGetProperty("errorCode", out var errorCodeProperty).Should().BeTrue();
            errorCodeProperty.GetString().Should().NotBeNullOrEmpty();
            
            response.TryGetProperty("timestamp", out var timestampProperty).Should().BeTrue();
            timestampProperty.GetDateTime().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        [Fact]
        public async Task InvokeAsync_ShouldLogErrorWithCorrectLevel()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Beklenmeyen bir hata oluştu")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithCustomBusinessException_ShouldUseCustomStatusCode()
        {
            // Arrange
            var customBusinessException = new BusinessException("Custom error", "CUSTOM_ERROR", 422);
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(customBusinessException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(422);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("CUSTOM_ERROR");
            responseBody.Should().Contain("Custom error");
        }

        [Fact]
        public async Task InvokeAsync_WithMultipleValidationErrors_ShouldCombineMessages()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Field1", "Error 1"),
                new ValidationFailure("Field2", "Error 2"),
                new ValidationFailure("Field3", "Error 3")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var message = jsonResponse.GetProperty("message").GetString();
            message.Should().Contain("Error 1");
            message.Should().Contain("Error 2");
            message.Should().Contain("Error 3");
            message.Should().Contain("Doğrulama hatası");
        }
    }
}
