using Xunit;
using Microsoft.AspNetCore.Http;
using Moq;
using FluentAssertions;
using System.Text.Json;
using SD_Turizm.API.Middleware;
using FluentValidation;
using FluentValidation.Results;

namespace SD_Turizm.Tests.Unit.Middleware
{
    public class ValidationMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly ValidationMiddleware _middleware;
        private readonly DefaultHttpContext _httpContext;

        public ValidationMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new ValidationMiddleware(_nextMock.Object);
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
            
            responseBody.Should().NotBeNullOrEmpty();
            responseBody.Should().Contain("Validation failed");
            responseBody.Should().Contain("Name");
            responseBody.Should().Contain("Name is required");
            responseBody.Should().Contain("Email");
            responseBody.Should().Contain("Email is invalid");
        }

        [Fact]
        public async Task InvokeAsync_WithValidationException_ShouldReturnCorrectJsonStructure()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Username", "Username is required"),
                new ValidationFailure("Password", "Password must be at least 6 characters")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            
            // Check main structure
            response.TryGetProperty("error", out var errorProperty).Should().BeTrue();
            errorProperty.GetString().Should().Be("Validation failed");
            
            response.TryGetProperty("details", out var detailsProperty).Should().BeTrue();
            detailsProperty.ValueKind.Should().Be(JsonValueKind.Array);
            
            var details = detailsProperty.EnumerateArray().ToList();
            details.Should().HaveCount(2);
            
            // Check first validation error
            var firstError = details[0];
            firstError.TryGetProperty("field", out var field1Property).Should().BeTrue();
            field1Property.GetString().Should().Be("Username");
            firstError.TryGetProperty("message", out var message1Property).Should().BeTrue();
            message1Property.GetString().Should().Be("Username is required");
            
            // Check second validation error
            var secondError = details[1];
            secondError.TryGetProperty("field", out var field2Property).Should().BeTrue();
            field2Property.GetString().Should().Be("Password");
            secondError.TryGetProperty("message", out var message2Property).Should().BeTrue();
            message2Property.GetString().Should().Be("Password must be at least 6 characters");
        }

        [Fact]
        public async Task InvokeAsync_WithSingleValidationError_ShouldHandleCorrectly()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email format is invalid")
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
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            
            response.TryGetProperty("details", out var detailsProperty).Should().BeTrue();
            var details = detailsProperty.EnumerateArray().ToList();
            details.Should().HaveCount(1);
            
            var error = details[0];
            error.TryGetProperty("field", out var fieldProperty).Should().BeTrue();
            fieldProperty.GetString().Should().Be("Email");
            error.TryGetProperty("message", out var messageProperty).Should().BeTrue();
            messageProperty.GetString().Should().Be("Email format is invalid");
        }

        [Fact]
        public async Task InvokeAsync_WithEmptyValidationErrors_ShouldHandleCorrectly()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>();
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);
            _httpContext.Response.ContentType.Should().Be("application/json");

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            
            response.TryGetProperty("error", out var errorProperty).Should().BeTrue();
            errorProperty.GetString().Should().Be("Validation failed");
            
            response.TryGetProperty("details", out var detailsProperty).Should().BeTrue();
            var details = detailsProperty.EnumerateArray().ToList();
            details.Should().HaveCount(0);
        }

        [Fact]
        public async Task InvokeAsync_WithNonValidationException_ShouldNotCatch()
        {
            // Arrange
            var genericException = new ArgumentException("Some other exception");
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(genericException);

            // Act & Assert
            var act = async () => await _middleware.InvokeAsync(_httpContext);
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Some other exception");
        }

        [Fact]
        public async Task InvokeAsync_WithValidationException_ShouldNotCallNextAgain()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Test", "Test error")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _nextMock.Verify(x => x(_httpContext), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithComplexValidationErrors_ShouldHandleSpecialCharacters()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Description", "Açıklama 'özel' karakterler içerebilir: ğüşıöçĞÜŞİÖÇ"),
                new ValidationFailure("Price", "Fiyat \"geçerli\" bir sayı olmalıdır & 0'dan büyük olmalıdır")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var details = jsonResponse.GetProperty("details").EnumerateArray().ToList();
            var messages = details.Select(d => d.GetProperty("message").GetString()).ToList();
            var combinedMessage = string.Join(" ", messages);
            combinedMessage.Should().Contain("Açıklama");
            combinedMessage.Should().Contain("özel");
            combinedMessage.Should().Contain("ğüşıöçĞÜŞİÖÇ");
            combinedMessage.Should().Contain("Fiyat");
            combinedMessage.Should().Contain("geçerli");
            combinedMessage.Should().Contain("0'dan büyük");
        }

        [Fact]
        public async Task InvokeAsync_WithLongValidationMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var longMessage = new string('A', 500) + " - This is a very long validation message";
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("LongField", longMessage)
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.StatusCode.Should().Be(400);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            responseBody.Should().Contain("LongField");
            responseBody.Should().Contain(longMessage);
        }

        [Fact]
        public async Task InvokeAsync_WithMultipleErrorsOnSameField_ShouldHandleCorrectly()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email is required"),
                new ValidationFailure("Email", "Email format is invalid"),
                new ValidationFailure("Email", "Email domain is not allowed")
            };
            var validationException = new FluentValidation.ValidationException(validationFailures);
            
            _nextMock.Setup(x => x(_httpContext)).ThrowsAsync(validationException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            
            var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
            response.TryGetProperty("details", out var detailsProperty).Should().BeTrue();
            var details = detailsProperty.EnumerateArray().ToList();
            details.Should().HaveCount(3);
            
            // All should be for Email field
            details.All(d => d.GetProperty("field").GetString() == "Email").Should().BeTrue();
            
            // Should contain all three messages
            responseBody.Should().Contain("Email is required");
            responseBody.Should().Contain("Email format is invalid");
            responseBody.Should().Contain("Email domain is not allowed");
        }
    }
}
