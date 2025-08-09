using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using SD_Turizm.Application.Validators;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Validators
{
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        [Fact]
        public void Should_Have_Error_When_PNRNumber_Is_Empty()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PNRNumber)
                .WithErrorMessage("PNR numarası boş olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_PNRNumber_Is_Too_Long()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = new string('A', 21), // 21 characters
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PNRNumber)
                .WithErrorMessage("PNR numarası 20 karakterden uzun olamaz");
        }

        [Theory]
        [InlineData("pnr123")] // lowercase
        [InlineData("PNR-123")] // with dash
        [InlineData("PNR 123")] // with space
        public void Should_Have_Error_When_PNRNumber_Has_Invalid_Characters(string pnrNumber)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = pnrNumber,
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PNRNumber)
                .WithErrorMessage("PNR numarası sadece büyük harf ve rakam içerebilir");
        }

        [Theory]
        [InlineData("PNR123")]
        [InlineData("ABC123DEF")]
        [InlineData("1234567890")]
        public void Should_Not_Have_Error_When_PNRNumber_Is_Valid(string pnrNumber)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = pnrNumber,
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PNRNumber);
        }

        [Fact]
        public void Should_Have_Error_When_CariCode_Is_Empty()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "",
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CariCode)
                .WithErrorMessage("Cari kodu boş olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_CariCode_Is_Too_Long()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = new string('A', 51), // 51 characters
                CustomerName = "Test Customer",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CariCode)
                .WithErrorMessage("Cari kodu 50 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_CustomerName_Is_Empty()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "",
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerName)
                .WithErrorMessage("Müşteri adı boş olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_CustomerName_Is_Too_Long()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = new string('A', 201), // 201 characters
                Currency = "TRY"
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CustomerName)
                .WithErrorMessage("Müşteri adı 200 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Currency_Is_Empty()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = ""
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Para birimi boş olamaz");
        }

        [Theory]
        [InlineData("TR")] // Too short
        [InlineData("TRYY")] // Too long
        public void Should_Have_Error_When_Currency_Length_Is_Invalid(string currency)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = currency
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Para birimi 3 karakter olmalıdır");
        }

        [Theory]
        [InlineData("try")] // lowercase
        [InlineData("TR1")] // with number
        [InlineData("T-Y")] // with special char
        public void Should_Have_Error_When_Currency_Has_Invalid_Format(string currency)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = currency
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Para birimi 3 büyük harf olmalıdır");
        }

        [Theory]
        [InlineData("TRY")]
        [InlineData("USD")]
        [InlineData("EUR")]
        public void Should_Not_Have_Error_When_Currency_Is_Valid(string currency)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = currency
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Currency);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Should_Have_Error_When_TotalAmount_Is_Negative(decimal totalAmount)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                TotalAmount = totalAmount
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TotalAmount)
                .WithErrorMessage("Toplam tutar negatif olamaz");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(1000.50)]
        public void Should_Not_Have_Error_When_TotalAmount_Is_Valid(decimal totalAmount)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                TotalAmount = totalAmount
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.TotalAmount);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Should_Have_Error_When_SalePrice_Is_Negative(decimal salePrice)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                SalePrice = salePrice
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SalePrice)
                .WithErrorMessage("Satış fiyatı negatif olamaz");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Should_Have_Error_When_PurchasePrice_Is_Negative(decimal purchasePrice)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                PurchasePrice = purchasePrice
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PurchasePrice)
                .WithErrorMessage("Alış fiyatı negatif olamaz");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Should_Have_Error_When_TotalAmountTL_Is_Negative(decimal totalAmountTL)
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                TotalAmountTL = totalAmountTL
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TotalAmountTL)
                .WithErrorMessage("Toplam tutar TL negatif olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_AgencyCode_Is_Too_Long()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                AgencyCode = new string('A', 51) // 51 characters
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AgencyCode)
                .WithErrorMessage("Acenta kodu 50 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Not_Have_Error_When_AgencyCode_Is_Empty()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                AgencyCode = ""
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AgencyCode);
        }

        [Fact]
        public void Should_Have_Error_When_SaleDate_Is_In_Future()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                SaleDate = DateTime.Now.AddDays(1) // Future date
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SaleDate)
                .WithErrorMessage("Satış tarihi gelecek bir tarih olamaz");
        }

        [Fact]
        public void Should_Not_Have_Error_When_SaleDate_Is_Valid()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR123", 
                CariCode = "CARI001",
                CustomerName = "Test Customer",
                Currency = "TRY",
                SaleDate = DateTime.Now.AddDays(-1) // Past date
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.SaleDate);
        }

        [Fact]
        public void Should_Not_Have_Any_Errors_When_All_Required_Fields_Are_Valid()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "PNR12345",
                CariCode = "CARI001",
                CustomerName = "John Doe",
                Currency = "TRY",
                TotalAmount = 1000,
                SalePrice = 1200,
                PurchasePrice = 1000,
                TotalAmountTL = 1000,
                AgencyCode = "AGENCY001",
                PackageCode = "PKG001",
                SellerType = "Online",
                FileCode = "FILE001",
                ProductName = "Hotel Package",
                SaleDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Multiple_Errors_When_Multiple_Fields_Are_Invalid()
        {
            // Arrange
            var sale = new Sale 
            { 
                PNRNumber = "", // Invalid
                CariCode = "", // Invalid
                CustomerName = "", // Invalid
                Currency = "TR", // Invalid
                TotalAmount = -100, // Invalid
                SaleDate = DateTime.Now.AddDays(1) // Invalid
            };

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PNRNumber);
            result.ShouldHaveValidationErrorFor(x => x.CariCode);
            result.ShouldHaveValidationErrorFor(x => x.CustomerName);
            result.ShouldHaveValidationErrorFor(x => x.Currency);
            result.ShouldHaveValidationErrorFor(x => x.TotalAmount);
            result.ShouldHaveValidationErrorFor(x => x.SaleDate);
        }
    }
}
