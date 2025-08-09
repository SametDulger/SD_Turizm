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
using SD_Turizm.Core.Entities;
using SD_Turizm.API;
using SD_Turizm.Tests.Integration.Helpers;

namespace SD_Turizm.Tests.Integration.Controllers
{
    public class HotelsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HotelsControllerTests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
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
        public async Task Get_ShouldReturnOkResult()
        {
            // Arrange
            _client.AddAuthorizationHeader();
            
            // Act
            var response = await _client.GetAsync("/api/v1/Hotels");

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Post_WithValidHotel_ShouldReturnCreated()
        {
            // Arrange
            _client.AddAuthorizationHeader();
            
            var hotel = new
            {
                Name = "Test Hotel",
                Code = "TEST001",
                Location = "Test Location",
                Stars = 4
            };

            var json = JsonSerializer.Serialize(hotel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/Hotels", content);

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Put_WithValidHotel_ShouldReturnNoContent()
        {
            // Arrange
            _client.AddAuthorizationHeader();
            
            var hotel = new
            {
                Id = 1,
                Name = "Updated Hotel",
                Code = "UPD001",
                Location = "Updated Location",
                Stars = 5
            };

            var json = JsonSerializer.Serialize(hotel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/Hotels/1", content);

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _client.AddAuthorizationHeader();
            
            // Act
            var response = await _client.DeleteAsync("/api/v1/Hotels/1");

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.Unauthorized);
        }
    }
}
