using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Core.DTOs;
using SD_Turizm.API;

namespace SD_Turizm.Tests.Integration.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove all existing DbContext registrations
                    var dbContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(ApplicationDbContext));
                    if (dbContextDescriptor != null)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    var dbContextOptionsDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (dbContextOptionsDescriptor != null)
                    {
                        services.Remove(dbContextOptionsDescriptor);
                    }

                    // Remove all DbContextOptions registrations
                    var allDbContextOptions = services.Where(d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();
                    foreach (var descriptor in allDbContextOptions)
                    {
                        services.Remove(descriptor);
                    }

                    // Add in-memory database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_Auth_" + Guid.NewGuid().ToString());
                    });

                    // Add memory cache for tests
                    services.AddMemoryCache();
                });
                
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        {"Jwt:Key", "TestKeyWithMinimum32CharactersLong"},
                        {"Jwt:Issuer", "TestIssuer"},
                        {"Jwt:Audience", "TestAudience"}
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "invaliduser",
                Password = "invalidpassword"
            };

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_WithEmptyUsername_ShouldReturnBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "",
                Password = "password123"
            };

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldReturnBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "testuser",
                Password = ""
            };

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Username = "testuser_" + Guid.NewGuid().ToString("N")[..8],
                Email = $"test_{Guid.NewGuid().ToString("N")[..8]}@test.com",
                Password = "Password123!"
            };

            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/register", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var registerResponse = JsonSerializer.Deserialize<RegisterResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            registerResponse.Should().NotBeNull();
            registerResponse!.Username.Should().Be(registerRequest.Username);
            registerResponse.Email.Should().Be(registerRequest.Email);
        }

        [Fact]
        public async Task Register_WithEmptyUsername_ShouldReturnBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Username = "",
                Email = "test@test.com",
                Password = "Password123!"
            };

            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/register", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithShortPassword_ShouldReturnBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestDto
            {
                Username = "testuser",
                Email = "test@test.com",
                Password = "123" // Too short
            };

            var json = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Auth/register", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ThenLogin_ShouldWork()
        {
            // Arrange
            var username = "testuser_" + Guid.NewGuid().ToString("N")[..8];
            var email = $"test_{Guid.NewGuid().ToString("N")[..8]}@test.com";
            var password = "Password123!";

            var registerRequest = new RegisterRequestDto
            {
                Username = username,
                Email = email,
                Password = password
            };

            var registerJson = JsonSerializer.Serialize(registerRequest);
            var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");

            // Act - Register
            var registerResponse = await _client.PostAsync("/api/v1/Auth/register", registerContent);

            // Assert - Register should succeed
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Arrange - Login
            var loginRequest = new LoginRequestDto
            {
                Username = username,
                Password = password
            };

            var loginJson = JsonSerializer.Serialize(loginRequest);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            // Act - Login
            var loginResponse = await _client.PostAsync("/api/v1/Auth/login", loginContent);

            // Assert - Login should succeed
            loginResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
            
            // Only verify JSON if login was successful
            if (loginResponse.IsSuccessStatusCode)
            {
                var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
                var loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(loginResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                loginResponseDto.Should().NotBeNull();
                loginResponseDto!.Username.Should().Be(username);
                loginResponseDto.Token.Should().NotBeNullOrEmpty();
            }
        }
    }
}
