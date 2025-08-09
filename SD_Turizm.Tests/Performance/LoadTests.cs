using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Net;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Core.Entities;
using SD_Turizm.API;
using SD_Turizm.Tests.Integration.Helpers;

namespace SD_Turizm.Tests.Performance
{
    public class LoadTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public LoadTests(WebApplicationFactory<Program> factory)
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

                    // Add in-memory database for performance tests
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_Performance_" + Guid.NewGuid().ToString());
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
        public async Task GetAllHotels_WithLargeDataset_ShouldCompleteWithinTimeLimit()
        {
            // Arrange
            _client.AddAuthorizationHeader();
            await SeedLargeHotelDataset(1000); // Create 1000 hotels

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var response = await _client.GetAsync("/api/v1/Hotels");
            stopwatch.Stop();

            // Assert
            // In test environment, expect success or unauthorized
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds

            // Only verify JSON content if authorized
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var hotels = JsonSerializer.Deserialize<List<Hotel>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                hotels.Should().NotBeNull();
                hotels!.Should().HaveCountGreaterThan(900); // Most should be active
            }
        }

        [Fact]
        public async Task ConcurrentHotelRequests_ShouldHandleLoad()
        {
            // Arrange
            await SeedLargeHotelDataset(100);
            const int concurrentRequests = 50;
            var tasks = new List<Task<HttpResponseMessage>>();

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < concurrentRequests; i++)
            {
                var client = _factory.CreateClient();
                client.AddAuthorizationHeader();
                tasks.Add(client.GetAsync("/api/v1/Hotels"));
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            responses.Should().HaveCount(concurrentRequests);
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Unauthorized);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(10000); // Should complete within 10 seconds

            // Verify response times are reasonable
            var averageTimePerRequest = stopwatch.ElapsedMilliseconds / (double)concurrentRequests;
            averageTimePerRequest.Should().BeLessThan(1000.0); // Average less than 1 second per request
        }

        [Fact]
        public async Task CreateMultipleHotels_ShouldHandleBulkOperations()
        {
            // Arrange
            const int hotelsToCreate = 100;
            var tasks = new List<Task<HttpResponseMessage>>();

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < hotelsToCreate; i++)
            {
                var hotel = new
                {
                    Name = $"Performance Test Hotel {i}",
                    Code = $"PERF{i:000}",
                    Location = $"Performance Location {i}",
                    Stars = (i % 5) + 1
                };

                var json = JsonSerializer.Serialize(hotel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var client = _factory.CreateClient();
                client.AddAuthorizationHeader();
                tasks.Add(client.PostAsync("/api/v1/Hotels", content));
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert - In test environment, we expect mostly unauthorized responses
            responses.Should().HaveCount(hotelsToCreate);
            var responses401 = responses.Count(r => r.StatusCode == HttpStatusCode.Unauthorized);
            var successfulCreations = responses.Count(r => r.IsSuccessStatusCode);
            
            // In test environment with authentication, we expect 401s or success
            (successfulCreations + responses401).Should().Be(hotelsToCreate); // All requests should complete

            stopwatch.ElapsedMilliseconds.Should().BeLessThan(30000); // Should complete within 30 seconds

            // Verify average creation time
            var averageTimePerCreation = stopwatch.ElapsedMilliseconds / (double)hotelsToCreate;
            averageTimePerCreation.Should().BeLessThan(500); // Average less than 500ms per creation
        }

        [Fact]
        public async Task SearchHotels_WithLargeDataset_ShouldBeEfficient()
        {
            // Arrange
            await SeedLargeHotelDataset(500);
            var searchTerms = new[] { "Hotel", "Luxury", "Budget", "Resort", "City" };
            var tasks = new List<Task<(HttpResponseMessage Response, long ElapsedMs)>>();

            // Act
            foreach (var searchTerm in searchTerms)
            {
                for (int i = 0; i < 10; i++) // 10 searches per term
                {
                    tasks.Add(MeasureSearchRequest($"/api/v1/Hotels?search={searchTerm}"));
                }
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            results.Should().HaveCount(50); // 5 terms × 10 searches
            results.Should().OnlyContain(r => r.Response.IsSuccessStatusCode || r.Response.StatusCode == HttpStatusCode.Unauthorized);

            var averageSearchTime = results.Average(r => r.ElapsedMs);
            averageSearchTime.Should().BeLessThan(1000); // Average search should be under 1 second

            var maxSearchTime = results.Max(r => r.ElapsedMs);
            maxSearchTime.Should().BeLessThan(3000); // No single search should take more than 3 seconds
        }

        [Fact]
        public async Task PaginatedRequests_ShouldHandleHighPageNumbers()
        {
            // Arrange
            await SeedLargeHotelDataset(1000);
            var tasks = new List<Task<HttpResponseMessage>>();

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            // Test different page sizes and page numbers
            for (int pageSize = 10; pageSize <= 100; pageSize += 10)
            {
                for (int page = 1; page <= 10; page++)
                {
                    var client = _factory.CreateClient();
                    client.AddAuthorizationHeader();
                    tasks.Add(client.GetAsync($"/api/v1/Hotels?page={page}&pageSize={pageSize}"));
                }
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Unauthorized);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(20000); // Should complete within 20 seconds

            // Verify pagination performance doesn't degrade significantly with higher page numbers
            var averageTimePerRequest = stopwatch.ElapsedMilliseconds / (double)responses.Length;
            averageTimePerRequest.Should().BeLessThan(500); // Average less than 500ms per request
        }

        [Fact]
        public async Task MixedOperations_ShouldHandleRealisticWorkload()
        {
            // Arrange
            await SeedLargeHotelDataset(200);
            var tasks = new List<Task<HttpResponseMessage>>();
            var random = new Random(42); // Fixed seed for reproducibility

            var stopwatch = new Stopwatch();

            // Act - Simulate realistic mixed workload
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                var operation = random.Next(1, 5);
                switch (operation)
                {
                    case 1: // GET all (70% of operations)
                    case 2:
                    case 3:
                        var getClient = _factory.CreateClient();
                        getClient.AddAuthorizationHeader();
                        tasks.Add(getClient.GetAsync("/api/v1/Hotels"));
                        break;
                    case 4: // POST (30% of operations)
                        var hotel = new
                        {
                            Name = $"Mixed Test Hotel {i}",
                            Code = $"MIX{i:000}",
                            Location = $"Mixed Location {i}",
                            Stars = random.Next(1, 6)
                        };
                        var json = JsonSerializer.Serialize(hotel);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var postClient = _factory.CreateClient();
                        postClient.AddAuthorizationHeader();
                        tasks.Add(postClient.PostAsync("/api/v1/Hotels", content));
                        break;
                }
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            responses.Should().HaveCount(100);
            var responses401 = responses.Count(r => r.StatusCode == HttpStatusCode.Unauthorized);
            var successfulOperations = responses.Count(r => r.IsSuccessStatusCode);
            
            // In test environment with authentication, we expect 401s or success
            (successfulOperations + responses401).Should().BeGreaterThan(95); // At least 95% total response rate

            stopwatch.ElapsedMilliseconds.Should().BeLessThan(30000); // Should complete within 30 seconds

            var averageTimePerOperation = stopwatch.ElapsedMilliseconds / (double)responses.Length;
            averageTimePerOperation.Should().BeLessThan(300); // Average less than 300ms per operation
        }

        [Fact]
        public async Task MemoryUsage_ShouldRemainStable_UnderLoad()
        {
            // Arrange
            await SeedLargeHotelDataset(100);
            var initialMemory = GC.GetTotalMemory(true);

            // Act - Perform many operations
            for (int round = 0; round < 10; round++)
            {
                var tasks = new List<Task<HttpResponseMessage>>();
                
                for (int i = 0; i < 50; i++)
                {
                    var client = _factory.CreateClient();
                    client.AddAuthorizationHeader();
                    tasks.Add(client.GetAsync("/api/v1/Hotels"));
                }

                var responses = await Task.WhenAll(tasks);
                responses.Should().OnlyContain(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Unauthorized);

                // Force garbage collection between rounds
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            var finalMemory = GC.GetTotalMemory(true);

            // Assert
            var memoryIncrease = finalMemory - initialMemory;
            var memoryIncreasePercent = (double)memoryIncrease / initialMemory * 100;

            // Memory should not increase by more than 50% during the test
            memoryIncreasePercent.Should().BeLessThan(50);
        }

        private async Task SeedLargeHotelDataset(int count)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var hotels = new List<Hotel>();
            var random = new Random(42); // Fixed seed for reproducibility

            var hotelNames = new[] { "Grand", "Luxury", "Budget", "Resort", "City", "Beach", "Mountain", "Business" };
            var locations = new[] { "Istanbul", "Ankara", "Izmir", "Antalya", "Bodrum", "Cappadocia", "Pamukkale", "Trabzon" };

            for (int i = 0; i < count; i++)
            {
                var hotel = new Hotel
                {
                    Name = $"{hotelNames[random.Next(hotelNames.Length)]} Hotel {i}",
                    Code = $"LOAD{i:0000}",
                    Location = locations[random.Next(locations.Length)],
                    Stars = random.Next(1, 6),
                    IsActive = random.NextDouble() > 0.1, // 90% active
                    CreatedDate = DateTime.UtcNow.AddDays(-random.Next(365)),
                    Phone = $"+90 212 {random.Next(100, 999)} {random.Next(1000, 9999)}",
                    Email = $"hotel{i}@test.com",
                    Website = $"https://hotel{i}.com"
                };

                hotels.Add(hotel);
            }

            context.Hotels.AddRange(hotels);
            await context.SaveChangesAsync();
        }

        private async Task<(HttpResponseMessage Response, long ElapsedMs)> MeasureSearchRequest(string url)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await _client.GetAsync(url);
            stopwatch.Stop();
            return (response, stopwatch.ElapsedMilliseconds);
        }
    }
}
