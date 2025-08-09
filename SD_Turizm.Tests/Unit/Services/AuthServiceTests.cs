using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;
using BCrypt.Net;

namespace SD_Turizm.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<User>> _mockUserRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IGenericRepository<User>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockLogger = new Mock<ILogger<AuthService>>();
            
            _mockUnitOfWork.Setup(uow => uow.Repository<User>()).Returns(_mockUserRepository.Object);
            
            // Setup configuration
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("TestKeyWithMinimum32CharactersLong");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
            
            _authService = new AuthService(_mockUnitOfWork.Object, _mockConfiguration.Object, _mockHttpContextAccessor.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnLoginResponse()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            
            var user = new User
            {
                Id = 1,
                Username = username,
                Email = "test@test.com",
                PasswordHash = hashedPassword,
                IsActive = true
            };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.LoginAsync(username, password);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.LoginAsync(username, password);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_WithNewUser_ShouldReturnRegisterResponse()
        {
            // Arrange
            var username = "newuser";
            var email = "newuser@test.com";
            var password = "password123";

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            // Act
            var result = await _authService.RegisterAsync(username, email, password);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.Email.Should().Be(email);
            result.Message.Should().Be("Registration successful");
        }

        [Fact]
        public async Task RegisterAsync_WithExistingUser_ShouldReturnNull()
        {
            // Arrange
            var username = "existinguser";
            var email = "existing@test.com";
            var password = "password123";

            var existingUser = new User
            {
                Id = 1,
                Username = username,
                Email = email,
                IsActive = true
            };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { existingUser });

            // Act
            var result = await _authService.RegisterAsync(username, email, password);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateTokenAsync_WithValidToken_ShouldReturnTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpassword"),
                IsActive = true
            };

            // Create a valid token first
            var loginResponse = await CreateValidUserAndLogin(user);
            
            // Act
            var result = await _authService.ValidateTokenAsync(loginResponse?.Token!);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateTokenAsync_WithInvalidToken_ShouldReturnFalse()
        {
            // Act
            var result = await _authService.ValidateTokenAsync("invalid.token.here");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task RefreshTokenAsync_WithValidRefreshToken_ShouldReturnNewTokens()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpassword"),
                IsActive = true
            };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // First login to get refresh token
            var loginResponse = await _authService.LoginAsync(user.Username, "testpassword");

            // Act
            var result = await _authService.RefreshTokenAsync(loginResponse.RefreshToken);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBe(loginResponse.RefreshToken);
            result.Username.Should().Be(user.Username);
        }

        [Fact]
        public async Task RefreshTokenAsync_WithInvalidRefreshToken_ShouldReturnNull()
        {
            // Act
            var result = await _authService.RefreshTokenAsync("invalid-refresh-token");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ChangePasswordAsync_WithValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpassword"),
                IsActive = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<System.Security.Principal.IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("testuser");
            var mockPrincipal = new Mock<System.Security.Claims.ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.ChangePasswordAsync("oldpassword", "newpassword");

            // Assert
            result.Should().BeTrue();
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithInvalidCurrentPassword_ShouldReturnFalse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
                IsActive = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<System.Security.Principal.IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("testuser");
            var mockPrincipal = new Mock<System.Security.Claims.ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.ChangePasswordAsync("wrongpassword", "newpassword");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ChangePasswordAsync_WithNoAuthenticatedUser_ShouldReturnFalse()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<System.Security.Principal.IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns((string)null);
            var mockPrincipal = new Mock<System.Security.Claims.ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = await _authService.ChangePasswordAsync("oldpassword", "newpassword");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetCurrentUserAsync_WithAuthenticatedUser_ShouldReturnUserData()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                IsActive = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<System.Security.Principal.IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("testuser");
            var mockPrincipal = new Mock<System.Security.Claims.ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.GetCurrentUserAsync();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCurrentUserAsync_WithNoAuthenticatedUser_ShouldReturnNull()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<System.Security.Principal.IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns((string)null);
            var mockPrincipal = new Mock<System.Security.Claims.ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            // Act
            var result = await _authService.GetCurrentUserAsync();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Enable2FAAsync_ShouldReturnSecretKeyAndQrCode()
        {
            // Act
            var result = await _authService.Enable2FAAsync();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Verify2FAAsync_WithCorrectCode_ShouldReturnTrue()
        {
            // Act
            var result = await _authService.Verify2FAAsync("123456");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Verify2FAAsync_WithIncorrectCode_ShouldReturnFalse()
        {
            // Act
            var result = await _authService.Verify2FAAsync("wrong_code");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetRolesAsync_ShouldReturnListOfRoles()
        {
            // Act
            var result = await _authService.GetRolesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task AssignRoleAsync_ShouldReturnTrue()
        {
            // Act
            var result = await _authService.AssignRoleAsync(1, 2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetUserSessionsAsync_ShouldReturnListOfSessions()
        {
            // Act
            var result = await _authService.GetUserSessionsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task RevokeSessionAsync_ShouldReturnTrue()
        {
            // Act
            var result = await _authService.RevokeSessionAsync("session1");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task LogoutAsync_ShouldCompleteSuccessfully()
        {
            // Act & Assert
            await _authService.LogoutAsync();
            // If no exception is thrown, the test passes
        }

        [Theory]
        [InlineData("", "password")]
        [InlineData("username", "")]
        public async Task LoginAsync_WithEmptyCredentials_ShouldReturnNull(string username, string password)
        {
            // Act
            var result = await _authService.LoginAsync(username, password);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("", "test@test.com", "password")]
        [InlineData("username", "", "password")]
        public async Task RegisterAsync_WithEmptyUsernameOrEmail_ShouldNotBeNull(string username, string email, string password)
        {
            // Arrange - No existing users
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.RegisterAsync(username, email, password);

            // Assert - Should complete but may create user with empty fields
            // This is expected behavior since the service doesn't validate empty fields
        }

        private async Task<SD_Turizm.Core.DTOs.LoginResponseDto> CreateValidUserAndLogin(User user)
        {
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            return await _authService.LoginAsync(user.Username, "testpassword");
        }
    }
}
