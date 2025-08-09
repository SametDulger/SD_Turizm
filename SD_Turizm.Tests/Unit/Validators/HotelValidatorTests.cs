using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using SD_Turizm.Application.Validators;
using SD_Turizm.Core.Entities;

namespace SD_Turizm.Tests.Unit.Validators
{
    public class HotelValidatorTests
    {
        private readonly HotelValidator _validator;

        public HotelValidatorTests()
        {
            _validator = new HotelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var hotel = new Hotel { Name = "", Code = "HOTEL001" };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Otel adı boş olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = new string('a', 201), // 201 characters
                Code = "HOTEL001" 
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Otel adı 200 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Valid()
        {
            // Arrange
            var hotel = new Hotel { Name = "Valid Hotel Name", Code = "HOTEL001" };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Empty()
        {
            // Arrange
            var hotel = new Hotel { Name = "Test Hotel", Code = "" };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Otel kodu boş olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Code_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = new string('A', 21) // 21 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Otel kodu 20 karakterden uzun olamaz");
        }

        [Theory]
        [InlineData("hotel001")] // lowercase
        [InlineData("HOTEL-001")] // with dash
        [InlineData("HOTEL 001")] // with space
        [InlineData("HOTEL_001")] // with underscore
        public void Should_Have_Error_When_Code_Has_Invalid_Characters(string code)
        {
            // Arrange
            var hotel = new Hotel { Name = "Test Hotel", Code = code };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Code)
                .WithErrorMessage("Otel kodu sadece büyük harf ve rakam içerebilir");
        }

        [Theory]
        [InlineData("HOTEL001")]
        [InlineData("ABC123")]
        [InlineData("H1")]
        public void Should_Not_Have_Error_When_Code_Is_Valid(string code)
        {
            // Arrange
            var hotel = new Hotel { Name = "Test Hotel", Code = code };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void Should_Have_Error_When_Location_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Location = new string('a', 201) // 201 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Location)
                .WithErrorMessage("Konum 200 karakterden uzun olamaz");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Stars_Is_Out_Of_Range(int stars)
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Stars = stars
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Stars)
                .WithErrorMessage("Yıldız sayısı 1-5 arasında olmalıdır");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void Should_Not_Have_Error_When_Stars_Is_Valid(int stars)
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Stars = stars
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Stars);
        }

        [Fact]
        public void Should_Have_Error_When_RoomTypes_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                RoomTypes = new string('a', 101) // 101 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RoomTypes)
                .WithErrorMessage("Oda tipleri 100 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_RoomLocations_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                RoomLocations = new string('a', 101) // 101 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RoomLocations)
                .WithErrorMessage("Oda konumları 100 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Amenities_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Amenities = new string('a', 501) // 501 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amenities)
                .WithErrorMessage("Özellikler 500 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Phone_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Phone = new string('1', 21) // 21 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Phone)
                .WithErrorMessage("Telefon numarası 20 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Email = "invalid-email"
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Geçerli bir e-posta adresi giriniz");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Email = "hotel@test.com"
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Email = ""
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Website_Is_Too_Long()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel", 
                Code = "HOTEL001",
                Website = new string('a', 201) // 201 characters
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Website)
                .WithErrorMessage("Web sitesi 200 karakterden uzun olamaz");
        }

        [Fact]
        public void Should_Not_Have_Any_Errors_When_All_Fields_Are_Valid()
        {
            // Arrange
            var hotel = new Hotel 
            { 
                Name = "Test Hotel",
                Code = "HOTEL001",
                Location = "Istanbul",
                Stars = 4,
                RoomTypes = "Standard, Deluxe",
                RoomLocations = "Sea View, City View",
                Amenities = "Pool, Spa, Restaurant",
                Phone = "+90 212 123 4567",
                Email = "hotel@test.com",
                Website = "https://www.testhotel.com"
            };

            // Act
            var result = _validator.TestValidate(hotel);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
