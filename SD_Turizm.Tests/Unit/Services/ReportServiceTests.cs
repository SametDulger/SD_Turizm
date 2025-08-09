using Xunit;
using Moq;
using FluentAssertions;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Sale>> _mockSaleRepository;
        private readonly Mock<IGenericRepository<ExchangeRate>> _mockExchangeRateRepository;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSaleRepository = new Mock<IGenericRepository<Sale>>();
            _mockExchangeRateRepository = new Mock<IGenericRepository<ExchangeRate>>();
            
            _mockUnitOfWork.Setup(uow => uow.Repository<Sale>()).Returns(_mockSaleRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<ExchangeRate>()).Returns(_mockExchangeRateRepository.Object);
            
            _reportService = new ReportService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetSalesReportAsync_WithDateRange_ShouldReturnFilteredSales()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    PNRNumber = "PNR001", 
                    CreatedDate = new DateTime(2024, 1, 15),
                    SalePrice = 1000,
                    PurchasePrice = 800,
                    Currency = "TRY"
                },
                new Sale 
                { 
                    Id = 2, 
                    PNRNumber = "PNR002", 
                    CreatedDate = new DateTime(2024, 1, 20),
                    SalePrice = 2000,
                    PurchasePrice = 1600,
                    Currency = "USD"
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetSalesReportAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeInDescendingOrder(s => s.CreatedDate);
        }

        [Fact]
        public async Task GetSalesReportAsync_WithFilters_ShouldApplyAllFilters()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);
            var sellerType = "Online";
            var currency = "USD";
            var pnrNumber = "PNR001";

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    PNRNumber = "PNR001", 
                    CreatedDate = new DateTime(2024, 1, 15),
                    SellerType = "Online",
                    Currency = "USD",
                    SalePrice = 1000
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetSalesReportAsync(startDate, endDate, sellerType, currency, pnrNumber);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().PNRNumber.Should().Be("PNR001");
            result.First().SellerType.Should().Be("Online");
            result.First().Currency.Should().Be("USD");
        }

        [Fact]
        public async Task GetSalesSummaryAsync_ShouldCalculateCorrectTotals()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    SellerType = "Online",
                    SalePrice = 1000,
                    PurchasePrice = 800,
                    CreatedDate = new DateTime(2024, 1, 15)
                },
                new Sale 
                { 
                    Id = 2, 
                    SellerType = "Retail",
                    SalePrice = 2000,
                    PurchasePrice = 1500,
                    CreatedDate = new DateTime(2024, 1, 20)
                },
                new Sale 
                { 
                    Id = 3, 
                    SellerType = "Online",
                    SalePrice = 1500,
                    PurchasePrice = 1200,
                    CreatedDate = new DateTime(2024, 1, 25)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetSalesSummaryAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            
            // Convert to dictionary for easier testing
            var summaryDict = result.GetType().GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(result));

            summaryDict["TotalSales"].Should().Be(3);
            summaryDict["TotalSaleAmount"].Should().Be(4500M); // 1000 + 2000 + 1500
            summaryDict["TotalPurchaseAmount"].Should().Be(3500M); // 800 + 1500 + 1200
            summaryDict["TotalProfit"].Should().Be(1000M); // 4500 - 3500
        }

        [Fact]
        public async Task GetFinancialReportAsync_WithTRYCurrency_ShouldNotApplyExchangeRate()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);
            var currency = "TRY";

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    SalePrice = 1000,
                    PurchasePrice = 800,
                    CreatedDate = new DateTime(2024, 1, 15)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetFinancialReportAsync(startDate, endDate, currency);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().SalePrice.Should().Be(1000); // Should remain unchanged
            result.First().PurchasePrice.Should().Be(800); // Should remain unchanged
        }

        [Fact]
        public async Task GetFinancialReportAsync_WithForeignCurrency_ShouldApplyExchangeRate()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);
            var currency = "USD";

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    SalePrice = 1000,
                    PurchasePrice = 800,
                    CreatedDate = new DateTime(2024, 1, 15)
                }
            };

            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    Currency = "USD",
                    Rate = 30.5M,
                    Date = new DateTime(2024, 1, 15)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            _mockExchangeRateRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<ExchangeRate, bool>>>()))
                .ReturnsAsync(exchangeRates);

            // Act
            var result = await _reportService.GetFinancialReportAsync(startDate, endDate, currency);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().SalePrice.Should().Be(30500); // 1000 * 30.5
            result.First().PurchasePrice.Should().Be(24400); // 800 * 30.5
        }

        [Fact]
        public async Task GetFinancialSummaryAsync_ShouldCalculateCorrectMetrics()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    SalePrice = 1000,
                    PurchasePrice = 800,
                    CreatedDate = new DateTime(2024, 1, 15)
                },
                new Sale 
                { 
                    Id = 2, 
                    SalePrice = 2000,
                    PurchasePrice = 1500,
                    CreatedDate = new DateTime(2024, 1, 20)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetFinancialSummaryAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            
            var summaryDict = result.GetType().GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(result));

            summaryDict["TotalRevenue"].Should().Be(3000M); // 1000 + 2000
            summaryDict["TotalCost"].Should().Be(2300M); // 800 + 1500
            summaryDict["TotalProfit"].Should().Be(700M); // 3000 - 2300
            
            // Profit margin: (700 / 3000) * 100 = 23.33%
            var profitMargin = (decimal)summaryDict["ProfitMargin"]!;
            profitMargin.Should().BeApproximately(23.33M, 0.01M);
        }

        [Fact]
        public async Task GetCustomerReportAsync_ShouldGroupByCariCodeAndCustomerName()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    CariCode = "CUST001",
                    CustomerName = "John Doe",
                    SalePrice = 1000,
                    CreatedDate = new DateTime(2024, 1, 15)
                },
                new Sale 
                { 
                    Id = 2, 
                    CariCode = "CUST001",
                    CustomerName = "John Doe",
                    SalePrice = 1500,
                    CreatedDate = new DateTime(2024, 1, 20)
                },
                new Sale 
                { 
                    Id = 3, 
                    CariCode = "CUST002",
                    CustomerName = "Jane Smith",
                    SalePrice = 2000,
                    CreatedDate = new DateTime(2024, 1, 25)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetCustomerReportAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2); // Two unique customers

            var customerReports = result.ToList();
            
            // Check first customer (John Doe)
            var johnDoeReport = customerReports.FirstOrDefault(c => 
                c.GetType().GetProperty("CariCode")?.GetValue(c)?.ToString() == "CUST001");
            johnDoeReport.Should().NotBeNull();

            // Check second customer (Jane Smith)
            var janeSmithReport = customerReports.FirstOrDefault(c => 
                c.GetType().GetProperty("CariCode")?.GetValue(c)?.ToString() == "CUST002");
            janeSmithReport.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCustomerReportAsync_WithCariCodeFilter_ShouldFilterCorrectly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);
            var cariCode = "CUST001";

            var sales = new List<Sale>
            {
                new Sale 
                { 
                    Id = 1, 
                    CariCode = "CUST001",
                    CustomerName = "John Doe",
                    SalePrice = 1000,
                    CreatedDate = new DateTime(2024, 1, 15)
                }
            };

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(sales);

            // Act
            var result = await _reportService.GetCustomerReportAsync(startDate, endDate, cariCode);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            
            var customerReport = result.First();
            var cariCodeProperty = customerReport.GetType().GetProperty("CariCode")?.GetValue(customerReport);
            cariCodeProperty?.ToString().Should().Be("CUST001");
        }

        [Fact]
        public async Task GetSalesReportAsync_WithEmptyResult_ShouldReturnEmptyCollection()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _reportService.GetSalesReportAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFinancialSummaryAsync_WithNoSales_ShouldReturnZeroValues()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);

            _mockSaleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Sale, bool>>>()))
                .ReturnsAsync(new List<Sale>());

            // Act
            var result = await _reportService.GetFinancialSummaryAsync(startDate, endDate);

            // Assert
            result.Should().NotBeNull();
            
            var summaryDict = result.GetType().GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(result));

            summaryDict["TotalRevenue"].Should().Be(0M);
            summaryDict["TotalCost"].Should().Be(0M);
            summaryDict["TotalProfit"].Should().Be(0M);
            summaryDict["ProfitMargin"].Should().Be(0M);
        }
    }
}
