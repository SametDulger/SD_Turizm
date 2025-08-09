using Xunit;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using SD_Turizm.Infrastructure.Repositories;
using SD_Turizm.Infrastructure.Data;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Repositories
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public void Repository_ShouldReturnSameInstanceForSameType()
        {
            // Act
            var repo1 = _unitOfWork.Repository<Hotel>();
            var repo2 = _unitOfWork.Repository<Hotel>();

            // Assert
            repo1.Should().NotBeNull();
            repo2.Should().NotBeNull();
            repo1.Should().BeSameAs(repo2);
        }

        [Fact]
        public void Repository_ShouldReturnDifferentInstancesForDifferentTypes()
        {
            // Act
            var hotelRepo = _unitOfWork.Repository<Hotel>();
            var userRepo = _unitOfWork.Repository<User>();

            // Assert
            hotelRepo.Should().NotBeNull();
            userRepo.Should().NotBeNull();
            hotelRepo.Should().NotBeSameAs(userRepo);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChangesToDatabase()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                Code = "TEST001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();

            // Act
            await hotelRepo.AddAsync(hotel);
            var changes = await _unitOfWork.SaveChangesAsync();

            // Assert
            changes.Should().BeGreaterThan(0);
            
            // Verify in database
            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "TEST001");
            savedHotel.Should().NotBeNull();
            savedHotel!.Name.Should().Be("Test Hotel");
        }

        [Fact]
        public async Task SaveChangesAsync_WithoutChanges_ShouldReturnZero()
        {
            // Act
            var changes = await _unitOfWork.SaveChangesAsync();

            // Assert
            changes.Should().Be(0);
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task BeginTransactionAsync_ShouldStartTransaction()
        {
            // Act
            await _unitOfWork.BeginTransactionAsync();

            // Assert
            _context.Database.CurrentTransaction.Should().NotBeNull();
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task CommitTransactionAsync_ShouldCommitChanges()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Transaction Hotel",
                Code = "TRANS001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await hotelRepo.AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Assert
            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "TRANS001");
            savedHotel.Should().NotBeNull();
            savedHotel!.Name.Should().Be("Transaction Hotel");
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task RollbackTransactionAsync_ShouldRevertChanges()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Rollback Hotel",
                Code = "ROLLBACK001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await hotelRepo.AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.RollbackTransactionAsync();

            // Assert
            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "ROLLBACK001");
            savedHotel.Should().BeNull();
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task CommitTransactionAsync_AfterException_ShouldRollback()
        {
            // Arrange
            var hotel1 = new Hotel
            {
                Name = "Hotel 1",
                Code = "HOTEL001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotel2 = new Hotel
            {
                Name = "Hotel 2",
                Code = "HOTEL001", // Same code to cause constraint violation
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();

            // Act & Assert
            await _unitOfWork.BeginTransactionAsync();
            await hotelRepo.AddAsync(hotel1);
            await hotelRepo.AddAsync(hotel2);

            var act = async () =>
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            };

            await act.Should().ThrowAsync<Exception>();

            // Verify no hotels were saved
            var savedHotels = await _context.Hotels.Where(h => h.Code == "HOTEL001").ToListAsync();
            savedHotels.Should().BeEmpty();
        }

        [Fact]
        public async Task MultipleRepositories_ShouldWorkWithSameUnitOfWork()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Multi Test Hotel",
                Code = "MULTI001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var user = new User
            {
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = "hashedpassword",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();
            var userRepo = _unitOfWork.Repository<User>();

            // Act
            await hotelRepo.AddAsync(hotel);
            await userRepo.AddAsync(user);
            var changes = await _unitOfWork.SaveChangesAsync();

            // Assert
            changes.Should().BeGreaterThan(1);

            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "MULTI001");
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");

            savedHotel.Should().NotBeNull();
            savedUser.Should().NotBeNull();
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task Transaction_WithMultipleRepositories_ShouldBeAtomic()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Atomic Hotel",
                Code = "ATOMIC001",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var user = new User
            {
                Username = "atomicuser",
                Email = "atomic@test.com",
                PasswordHash = "hashedpassword",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();
            var userRepo = _unitOfWork.Repository<User>();

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await hotelRepo.AddAsync(hotel);
            await userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Assert
            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "ATOMIC001");
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "atomicuser");

            savedHotel.Should().NotBeNull();
            savedUser.Should().NotBeNull();
        }

        [Fact(Skip = "InMemory database doesn't support transactions")]
        public async Task Transaction_WithRollback_ShouldRevertAllChanges()
        {
            // Arrange
            var hotel = new Hotel
            {
                Name = "Rollback Hotel",
                Code = "ROLLBACK002",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var user = new User
            {
                Username = "rollbackuser",
                Email = "rollback@test.com",
                PasswordHash = "hashedpassword",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var hotelRepo = _unitOfWork.Repository<Hotel>();
            var userRepo = _unitOfWork.Repository<User>();

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await hotelRepo.AddAsync(hotel);
            await userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.RollbackTransactionAsync();

            // Assert
            var savedHotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Code == "ROLLBACK002");
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "rollbackuser");

            savedHotel.Should().BeNull();
            savedUser.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            // Act & Assert
            var act = () => _unitOfWork.Dispose();
            act.Should().NotThrow();
        }

        [Fact]
        public async Task RollbackTransactionAsync_WithoutTransaction_ShouldNotThrow()
        {
            // Act & Assert
            var act = async () => await _unitOfWork.RollbackTransactionAsync();
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task CommitTransactionAsync_WithoutTransaction_ShouldNotThrow()
        {
            // Act & Assert
            var act = async () => await _unitOfWork.CommitTransactionAsync();
            await act.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _context.Dispose();
        }
    }
}
