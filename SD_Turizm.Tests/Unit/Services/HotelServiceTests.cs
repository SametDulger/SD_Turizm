using Xunit;
using Moq;
using FluentAssertions;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Tests.Unit.Services
{
    public class HotelServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Hotel>> _mockRepository;
        private readonly HotelService _hotelService;

        public HotelServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IGenericRepository<Hotel>>();
            _mockUnitOfWork.Setup(uow => uow.Repository<Hotel>()).Returns(_mockRepository.Object);
            _hotelService = new HotelService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllHotelsAsync_ShouldReturnAllHotels()
        {
            // Arrange
            var expectedHotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Test Hotel 1", Code = "HOTEL001" },
                new Hotel { Id = 2, Name = "Test Hotel 2", Code = "HOTEL002" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedHotels);

            // Act
            var result = await _hotelService.GetAllHotelsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedHotels);
        }

        [Fact]
        public async Task GetHotelByIdAsync_WithValidId_ShouldReturnHotel()
        {
            // Arrange
            var hotelId = 1;
            var expectedHotel = new Hotel { Id = hotelId, Name = "Test Hotel", Code = "HOTEL001" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync(expectedHotel);

            // Act
            var result = await _hotelService.GetHotelByIdAsync(hotelId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedHotel);
        }

        [Fact]
        public async Task GetHotelByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var hotelId = 999;
            _mockRepository.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync((Hotel?)null);

            // Act
            var result = await _hotelService.GetHotelByIdAsync(hotelId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateHotelAsync_ShouldAddHotelAndSaveChanges()
        {
            // Arrange
            var hotel = new Hotel { Name = "New Hotel", Code = "HOTEL003" };

            // Act
            var result = await _hotelService.CreateHotelAsync(hotel);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(hotel), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            result.Should().BeEquivalentTo(hotel);
        }

        [Fact]
        public async Task UpdateHotelAsync_ShouldUpdateHotelAndSaveChanges()
        {
            // Arrange
            var hotel = new Hotel { Id = 1, Name = "Updated Hotel", Code = "HOTEL001" };

            // Act
            await _hotelService.UpdateHotelAsync(hotel);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(hotel), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteHotelAsync_ShouldDeleteHotelAndSaveChanges()
        {
            // Arrange
            var hotelId = 1;

            // Act
            await _hotelService.DeleteHotelAsync(hotelId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(hotelId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task HotelExistsAsync_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            var hotelId = 1;
            _mockRepository.Setup(repo => repo.ExistsAsync(hotelId)).ReturnsAsync(true);

            // Act
            var result = await _hotelService.HotelExistsAsync(hotelId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task HotelExistsAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var hotelId = 999;
            _mockRepository.Setup(repo => repo.ExistsAsync(hotelId)).ReturnsAsync(false);

            // Act
            var result = await _hotelService.HotelExistsAsync(hotelId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
