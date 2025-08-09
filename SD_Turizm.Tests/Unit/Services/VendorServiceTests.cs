using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Tests.Unit.Services
{
    public class VendorServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Vendor>> _mockVendorRepository;
        private readonly VendorService _vendorService;

        public VendorServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockVendorRepository = new Mock<IGenericRepository<Vendor>>();
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Vendor>()).Returns(_mockVendorRepository.Object);
            
            _vendorService = new VendorService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllVendorsAsync_ShouldReturnAllVendorsAsDto()
        {
            // Arrange
            var vendors = new List<Vendor>
            {
                new Hotel { Id = 1, Code = "HOTEL001", Name = "Test Hotel", IsActive = true },
                new Cruise { Id = 2, Code = "CRUISE001", Name = "Test Cruise", IsActive = true }
            };

            _mockVendorRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(vendors);

            // Act
            var result = await _vendorService.GetAllVendorsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            var resultList = result.ToList();
            resultList[0].Code.Should().Be("HOTEL001");
            resultList[0].Name.Should().Be("Test Hotel");
            resultList[1].Code.Should().Be("CRUISE001");
            resultList[1].Name.Should().Be("Test Cruise");
        }

        [Fact]
        public async Task GetVendorByIdAsync_WithValidId_ShouldReturnVendorDto()
        {
            // Arrange
            var vendorId = 1;
            var vendor = new Hotel 
            { 
                Id = vendorId, 
                Code = "HOTEL001", 
                Name = "Test Hotel",
                Email = "hotel@test.com",
                Phone = "123456789",
                IsActive = true
            };

            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(vendorId)).ReturnsAsync(vendor);

            // Act
            var result = await _vendorService.GetVendorByIdAsync(vendorId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(vendorId);
            result.Code.Should().Be("HOTEL001");
            result.Name.Should().Be("Test Hotel");
            result.Email.Should().Be("hotel@test.com");
            result.Phone.Should().Be("123456789");
            result.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetVendorByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var vendorId = 999;
            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(vendorId)).ReturnsAsync((Vendor?)null);

            // Act
            var result = await _vendorService.GetVendorByIdAsync(vendorId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetActiveVendorsAsync_ShouldReturnOnlyActiveVendors()
        {
            // Arrange
            var vendors = new List<Vendor>
            {
                new Hotel { Id = 1, Code = "HOTEL001", Name = "Active Hotel", IsActive = true },
                new Hotel { Id = 2, Code = "HOTEL002", Name = "Inactive Hotel", IsActive = false },
                new Cruise { Id = 3, Code = "CRUISE001", Name = "Active Cruise", IsActive = true }
            };

            _mockVendorRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(vendors);

            // Act
            var result = await _vendorService.GetActiveVendorsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(v => v.IsActive).Should().BeTrue();
        }

        [Fact]
        public async Task GetVendorsByTypeAsync_ShouldReturnVendorsOfSpecificType()
        {
            // Arrange
            var vendors = new List<Vendor>
            {
                new Hotel { Id = 1, Code = "HOTEL001", Name = "Hotel 1", IsActive = true },
                new Hotel { Id = 2, Code = "HOTEL002", Name = "Hotel 2", IsActive = true },
                new Cruise { Id = 3, Code = "CRUISE001", Name = "Cruise 1", IsActive = true }
            };

            _mockVendorRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(vendors);

            // Act
            var result = await _vendorService.GetVendorsByTypeAsync("Hotel");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(v => v.VendorType == "Hotel").Should().BeTrue();
        }

        [Fact]
        public async Task CreateVendorAsync_ShouldCreateVendorAndReturnDto()
        {
            // Arrange
            var vendorDto = new VendorDto
            {
                Code = "NEW001",
                Name = "New Vendor",
                Email = "new@vendor.com",
                Phone = "987654321",
                Address = "New Address",
                Country = "Turkey",
                Description = "New vendor description",
                IsActive = true
            };

            var createdVendor = new Hotel
            {
                Id = 1,
                Code = vendorDto.Code,
                Name = vendorDto.Name,
                Email = vendorDto.Email,
                Phone = vendorDto.Phone,
                Address = vendorDto.Address,
                Country = vendorDto.Country,
                Description = vendorDto.Description,
                IsActive = vendorDto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _mockVendorRepository.Setup(repo => repo.AddAsync(It.IsAny<Vendor>()))
                .ReturnsAsync(createdVendor);

            // Act
            var result = await _vendorService.CreateVendorAsync(vendorDto);

            // Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(vendorDto.Code);
            result.Name.Should().Be(vendorDto.Name);
            result.Email.Should().Be(vendorDto.Email);
            result.IsActive.Should().Be(vendorDto.IsActive);
            
            _mockVendorRepository.Verify(repo => repo.AddAsync(It.IsAny<Vendor>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateVendorAsync_WithExistingVendor_ShouldUpdateAndReturnDto()
        {
            // Arrange
            var existingVendor = new Hotel
            {
                Id = 1,
                Code = "OLD001",
                Name = "Old Name",
                Email = "old@vendor.com",
                IsActive = true
            };

            var vendorDto = new VendorDto
            {
                Id = 1,
                Code = "UPD001",
                Name = "Updated Name",
                Email = "updated@vendor.com",
                Phone = "111222333",
                IsActive = false
            };

            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingVendor);

            // Act
            var result = await _vendorService.UpdateVendorAsync(vendorDto);

            // Assert
            result.Should().NotBeNull();
            result!.Code.Should().Be(vendorDto.Code);
            result.Name.Should().Be(vendorDto.Name);
            result.Email.Should().Be(vendorDto.Email);
            result.IsActive.Should().Be(vendorDto.IsActive);
            
            _mockVendorRepository.Verify(repo => repo.UpdateAsync(existingVendor), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateVendorAsync_WithNonExistingVendor_ShouldReturnNull()
        {
            // Arrange
            var vendorDto = new VendorDto { Id = 999, Name = "Non-existing" };

            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Vendor?)null);

            // Act
            var result = await _vendorService.UpdateVendorAsync(vendorDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteVendorAsync_ShouldDeleteVendorAndReturnTrue()
        {
            // Arrange
            var vendorId = 1;

            // Act
            var result = await _vendorService.DeleteVendorAsync(vendorId);

            // Assert
            result.Should().BeTrue();
            _mockVendorRepository.Verify(repo => repo.DeleteAsync(vendorId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ToggleVendorStatusAsync_WithExistingVendor_ShouldToggleStatusAndReturnDto()
        {
            // Arrange
            var vendor = new Hotel
            {
                Id = 1,
                Code = "HOTEL001",
                Name = "Test Hotel",
                IsActive = true
            };

            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(vendor);

            // Act
            var result = await _vendorService.ToggleVendorStatusAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.IsActive.Should().BeFalse(); // Should be toggled
            vendor.IsActive.Should().BeFalse();
            
            _mockVendorRepository.Verify(repo => repo.UpdateAsync(vendor), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ToggleVendorStatusAsync_WithNonExistingVendor_ShouldReturnNull()
        {
            // Arrange
            _mockVendorRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Vendor?)null);

            // Act
            var result = await _vendorService.ToggleVendorStatusAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetVendorStatisticsAsync_ShouldReturnStatistics()
        {
            // Arrange
            var vendors = new List<Vendor>
            {
                new Hotel { Id = 1, IsActive = true },
                new Hotel { Id = 2, IsActive = false },
                new Cruise { Id = 3, IsActive = true },
                new RentACar { Id = 4, IsActive = true }
            };

            _mockVendorRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(vendors);

            // Act
            var result = await _vendorService.GetVendorStatisticsAsync();

            // Assert
            result.Should().NotBeNull();
            
            // Convert to dictionary for easier testing
            var statsDict = result.GetType().GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(result));
                
            statsDict.Should().NotBeEmpty();
        }

        [Fact(Skip = "EF Core async operations not supported with mock IQueryable")]
        public async Task GetVendorsWithPaginationAsync_ShouldCallRepository()
        {
            // Arrange
            var vendors = new List<Vendor>
            {
                new Hotel { Id = 1, Code = "HOTEL001", Name = "Hotel One", IsActive = true },
                new Hotel { Id = 2, Code = "HOTEL002", Name = "Hotel Two", IsActive = true }
            };

            // Mock the queryable directly without async complexity
            var mockQueryable = vendors.AsQueryable();
            _mockVendorRepository.Setup(repo => repo.GetQueryable()).Returns(mockQueryable);

            // Act & Assert - Just verify the method can be called without throwing
            var act = async () => await _vendorService.GetVendorsWithPaginationAsync(1, 2);
            await act.Should().NotThrowAsync();
        }
    }
}
