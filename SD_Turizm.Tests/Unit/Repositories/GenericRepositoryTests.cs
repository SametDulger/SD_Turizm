using Xunit;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using SD_Turizm.Infrastructure.Repositories;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Repositories
{
    public class GenericRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericRepository<Hotel> _repository;

        public GenericRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new GenericRepository<Hotel>(_context);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnEntity()
        {
            // Arrange
            var hotel = new Hotel
            {
                Id = 1,
                Name = "Test Hotel",
                Code = "HOTEL001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Hotel");
            result.Code.Should().Be("HOTEL001");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveEntities()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Active Hotel 1", Code = "H001", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 2, Name = "Active Hotel 2", Code = "H002", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 3, Name = "Inactive Hotel", Code = "H003", IsActive = false, CreatedDate = DateTime.UtcNow }
            };

            _context.Hotels.AddRange(hotels);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(h => h.IsActive).Should().BeTrue();
        }

        [Fact]
        public async Task FindAsync_WithPredicate_ShouldReturnMatchingActiveEntities()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Luxury Hotel", Code = "LUX001", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 2, Name = "Budget Hotel", Code = "BUD001", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 3, Name = "Luxury Resort", Code = "LUX002", IsActive = false, CreatedDate = DateTime.UtcNow }
            };

            _context.Hotels.AddRange(hotels);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(h => h.Name.Contains("Luxury"));

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1); // Only active luxury hotel
            result.First().Name.Should().Be("Luxury Hotel");
        }

        [Fact]
        public async Task AddAsync_ShouldSetCreatedDateAndIsActive()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "New Hotel",
                Code = "NEW001"
            };

            // Act
            var result = await _repository.AddAsync(hotel);
            await _context.SaveChangesAsync();

            // Assert
            result.Should().NotBeNull();
            result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            result.IsActive.Should().BeTrue();

            // Verify it's in database
            var savedHotel = await _context.Hotels.FindAsync(result.Id);
            savedHotel.Should().NotBeNull();
            savedHotel!.Name.Should().Be("New Hotel");
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetUpdatedDate()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Original Hotel",
                Code = "ORIG001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            hotel.Name = "Updated Hotel";
            await _repository.UpdateAsync(hotel);
            await _context.SaveChangesAsync();

            // Assert
            hotel.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            // Verify in database
            var updatedHotel = await _context.Hotels.FindAsync(hotel.Id);
            updatedHotel.Should().NotBeNull();
            updatedHotel!.Name.Should().Be("Updated Hotel");
            updatedHotel.UpdatedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetIsActiveFalseAndUpdatedDate()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Hotel To Delete",
                Code = "DEL001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            var hotelId = hotel.Id;

            // Act
            await _repository.DeleteAsync(hotelId);
            await _context.SaveChangesAsync();

            // Assert
            var deletedHotel = await _context.Hotels.FindAsync(hotelId);
            deletedHotel.Should().NotBeNull();
            deletedHotel!.IsActive.Should().BeFalse();
            deletedHotel.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            // Should not appear in GetAll
            var allHotels = await _repository.GetAllAsync();
            allHotels.Should().NotContain(h => h.Id == hotelId);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingId_ShouldNotThrow()
        {
            // Act & Assert
            var act = async () => await _repository.DeleteAsync(999);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ExistsAsync_WithExistingActiveEntity_ShouldReturnTrue()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Existing Hotel",
                Code = "EXIST001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(hotel.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WithInactiveEntity_ShouldReturnFalse()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Inactive Hotel",
                Code = "INACTIVE001",
                IsActive = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(hotel.Id);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Query_ShouldReturnOnlyActiveEntitiesAsQueryable()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Active Hotel 1", Code = "A001", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 2, Name = "Active Hotel 2", Code = "A002", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 3, Name = "Inactive Hotel", Code = "I001", IsActive = false, CreatedDate = DateTime.UtcNow }
            };

            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();

            // Act
            var query = _repository.Query();
            var result = query.ToList();

            // Assert
            result.Should().HaveCount(2);
            result.All(h => h.IsActive).Should().BeTrue();
        }

        [Fact]
        public void GetQueryable_ShouldReturnAllEntitiesAsQueryable()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Active Hotel", Code = "A001", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Hotel { Id = 2, Name = "Inactive Hotel", Code = "I001", IsActive = false, CreatedDate = DateTime.UtcNow }
            };

            _context.Hotels.AddRange(hotels);
            _context.SaveChanges();

            // Act
            var query = _repository.GetQueryable();
            var result = query.ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(h => h.IsActive == true);
            result.Should().Contain(h => h.IsActive == false);
        }

        [Fact]
        public async Task AddAsync_MultipleEntities_ShouldAllHaveCreatedDate()
        {
            // Arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "Hotel 1", Code = "H001" },
                new Hotel { Name = "Hotel 2", Code = "H002" },
                new Hotel { Name = "Hotel 3", Code = "H003" }
            };

            // Act
            foreach (var hotel in hotels)
            {
                await _repository.AddAsync(hotel);
            }
            await _context.SaveChangesAsync();

            // Assert
            var savedHotels = await _repository.GetAllAsync();
            savedHotels.Should().HaveCount(3);
            savedHotels.All(h => h.CreatedDate <= DateTime.UtcNow).Should().BeTrue();
            savedHotels.All(h => h.IsActive).Should().BeTrue();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
