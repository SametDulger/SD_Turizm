using Xunit;
using Moq;
using FluentAssertions;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Sale>> _mockSaleRepository;
        private readonly Mock<IGenericRepository<SaleItem>> _mockSaleItemRepository;
        private readonly Mock<IGenericRepository<SalePerson>> _mockSalePersonRepository;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSaleRepository = new Mock<IGenericRepository<Sale>>();
            _mockSaleItemRepository = new Mock<IGenericRepository<SaleItem>>();
            _mockSalePersonRepository = new Mock<IGenericRepository<SalePerson>>();
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Sale>()).Returns(_mockSaleRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<SaleItem>()).Returns(_mockSaleItemRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<SalePerson>()).Returns(_mockSalePersonRepository.Object);
            
            _saleService = new SaleService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllSalesAsync_ShouldReturnAllSales()
        {
            // Arrange
            var expectedSales = new List<Sale>
            {
                new Sale { Id = 1, PNRNumber = "PNR001", CustomerName = "Customer 1", TotalAmount = 1000 },
                new Sale { Id = 2, PNRNumber = "PNR002", CustomerName = "Customer 2", TotalAmount = 2000 }
            };

            _mockSaleRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedSales);

            // Act
            var result = await _saleService.GetAllSalesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedSales);
        }

        [Fact]
        public async Task GetSaleByIdAsync_WithValidId_ShouldReturnSale()
        {
            // Arrange
            var saleId = 1;
            var expectedSale = new Sale { Id = saleId, PNRNumber = "PNR001", CustomerName = "Customer 1" };

            _mockSaleRepository.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync(expectedSale);

            // Act
            var result = await _saleService.GetSaleByIdAsync(saleId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedSale);
        }

        [Fact]
        public async Task GetSaleByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var saleId = 999;
            _mockSaleRepository.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync((Sale?)null);

            // Act
            var result = await _saleService.GetSaleByIdAsync(saleId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateSaleAsync_ShouldAddSaleAndSaveChanges()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR003", 
                CustomerName = "New Customer", 
                TotalAmount = 1500,
                Currency = "EUR",
                SaleDate = DateTime.UtcNow
            };

            _mockSaleRepository.Setup(repo => repo.AddAsync(sale)).ReturnsAsync(sale);

            // Act
            var result = await _saleService.CreateSaleAsync(sale);

            // Assert
            _mockSaleRepository.Verify(repo => repo.AddAsync(sale), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            result.Should().BeEquivalentTo(sale);
        }

        [Fact]
        public async Task UpdateSaleAsync_ShouldUpdateSaleAndSaveChanges()
        {
            // Arrange
            var sale = new Sale 
            { 
                Id = 1, 
                PNRNumber = "PNR001", 
                CustomerName = "Updated Customer",
                TotalAmount = 2500
            };

            // Act
            await _saleService.UpdateSaleAsync(sale);

            // Assert
            _mockSaleRepository.Verify(repo => repo.UpdateAsync(sale), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteSaleAsync_ShouldDeleteSaleAndSaveChanges()
        {
            // Arrange
            var saleId = 1;

            // Act
            await _saleService.DeleteSaleAsync(saleId);

            // Assert
            _mockSaleRepository.Verify(repo => repo.DeleteAsync(saleId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SaleExistsAsync_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            var saleId = 1;
            _mockSaleRepository.Setup(repo => repo.ExistsAsync(saleId)).ReturnsAsync(true);

            // Act
            var result = await _saleService.SaleExistsAsync(saleId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SaleExistsAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var saleId = 999;
            _mockSaleRepository.Setup(repo => repo.ExistsAsync(saleId)).ReturnsAsync(false);

            // Act
            var result = await _saleService.SaleExistsAsync(saleId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetSalesByPNRAsync_ShouldReturnSaleByPNR()
        {
            // Arrange
            var pnrNumber = "PNR001";
            var expectedSale = new Sale { Id = 1, PNRNumber = pnrNumber, CustomerName = "John Doe" };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(new List<Sale> { expectedSale });

            // Act
            var result = await _saleService.GetSaleByPNRAsync(pnrNumber);

            // Assert
            result.Should().NotBeNull();
            result!.PNRNumber.Should().Be(pnrNumber);
        }
    }
}
