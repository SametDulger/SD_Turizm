using Xunit;
using Moq;
using FluentAssertions;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<User>> _mockUserRepository;
        private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
        private readonly Mock<IGenericRepository<UserRole>> _mockUserRoleRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IGenericRepository<User>>();
            _mockRoleRepository = new Mock<IGenericRepository<Role>>();
            _mockUserRoleRepository = new Mock<IGenericRepository<UserRole>>();
            
            _mockUnitOfWork.Setup(uow => uow.Repository<User>()).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<Role>()).Returns(_mockRoleRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<UserRole>()).Returns(_mockUserRoleRepository.Object);
            
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@test.com" },
                new User { Id = 2, Username = "user2", Email = "user2@test.com" }
            };

            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, Username = "testuser", Email = "test@test.com" };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var userId = 999;
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAsync_WithExistingUsername_ShouldReturnUser()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new User { Id = 1, Username = username, Email = "test@test.com" };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            // Act
            var result = await _userService.GetByUsernameAsync(username);

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be(username);
        }

        [Fact]
        public async Task GetByUsernameAsync_WithNonExistingUsername_ShouldReturnNull()
        {
            // Arrange
            var username = "nonexistentuser";

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetByUsernameAsync(username);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
        {
            // Arrange
            var email = "test@test.com";
            var expectedUser = new User { Id = 1, Username = "testuser", Email = email };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { expectedUser });

            // Act
            var result = await _userService.GetByEmailAsync(email);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(email);
        }

        [Fact]
        public async Task CreateAsync_WithValidUser_ShouldCreateUser()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                Email = "newuser@test.com",
                FirstName = "New",
                LastName = "User"
            };
            var password = "password123";

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>()); // No existing users

            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            // Act
            var result = await _userService.CreateAsync(user, password);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(user.Username);
            result.Email.Should().Be(user.Email);
            result.IsActive.Should().BeTrue();
            result.PasswordHash.Should().NotBeNullOrEmpty();
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithExistingUsername_ShouldThrowException()
        {
            // Arrange
            var user = new User
            {
                Username = "existinguser",
                Email = "new@test.com"
            };
            var password = "password123";

            var existingUser = new User { Username = "existinguser", Email = "existing@test.com" };
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { existingUser });

            // Act & Assert
            var act = async () => await _userService.CreateAsync(user, password);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Bu kullanıcı adı zaten kullanılıyor.");
        }

        [Fact]
        public async Task CreateAsync_WithExistingEmail_ShouldThrowException()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                Email = "existing@test.com"
            };
            var password = "password123";

            // First call for username check - returns empty
            // Second call for email check - returns existing user
            _mockUserRepository.SetupSequence(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>()) // Username check
                .ReturnsAsync(new List<User> { new User { Email = "existing@test.com" } }); // Email check

            // Act & Assert
            var act = async () => await _userService.CreateAsync(user, password);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Bu e-posta adresi zaten kullanılıyor.");
        }

        [Fact]
        public async Task UpdateAsync_WithValidUser_ShouldUpdateUser()
        {
            // Arrange
            var existingUser = new User
            {
                Id = 1,
                Username = "oldusername",
                Email = "old@test.com",
                PasswordHash = "oldhash"
            };

            var updatedUser = new User
            {
                Id = 1,
                Username = "newusername",
                Email = "new@test.com",
                FirstName = "Updated",
                LastName = "User"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>()); // No conflicts

            // Act
            var result = await _userService.UpdateAsync(updatedUser);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(updatedUser.Username);
            result.Email.Should().Be(updatedUser.Email);
            result.PasswordHash.Should().Be(existingUser.PasswordHash); // Password should remain unchanged
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingUser_ShouldThrowException()
        {
            // Arrange
            var user = new User { Id = 999, Username = "nonexistent" };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((User?)null);

            // Act & Assert
            var act = async () => await _userService.UpdateAsync(user);
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Kullanıcı bulunamadı.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUserAndSaveChanges()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var currentPassword = "oldpassword";
            var newPassword = "newpassword";

            var user = new User
            {
                Id = userId,
                Username = "testuser",
                PasswordHash = HashPassword(currentPassword) // Simulate hashed password
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);

            // Assert
            result.Should().BeTrue();
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithInvalidCurrentPassword_ShouldReturnFalse()
        {
            // Arrange
            var userId = 1;
            var currentPassword = "wrongpassword";
            var newPassword = "newpassword";

            var user = new User
            {
                Id = userId,
                Username = "testuser",
                PasswordHash = HashPassword("correctpassword")
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ChangePasswordAsync_WithNonExistingUser_ShouldReturnFalse()
        {
            // Arrange
            var userId = 999;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ChangePasswordAsync(userId, "oldpass", "newpass");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ResetPasswordAsync_WithValidEmail_ShouldReturnTrue()
        {
            // Arrange
            var email = "test@test.com";
            var user = new User { Id = 1, Email = email, Username = "testuser" };

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userService.ResetPasswordAsync(email);

            // Assert
            result.Should().BeTrue();
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_WithInvalidEmail_ShouldReturnFalse()
        {
            // Arrange
            var email = "nonexistent@test.com";

            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.ResetPasswordAsync(email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ActivateUserAsync_WithValidUserId_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, IsActive = false };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.ActivateUserAsync(userId);

            // Assert
            result.Should().BeTrue();
            user.IsActive.Should().BeTrue();
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateUserAsync_WithValidUserId_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, IsActive = true };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeactivateUserAsync(userId);

            // Assert
            result.Should().BeTrue();
            user.IsActive.Should().BeFalse();
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AssignRoleAsync_WithValidUserAndRole_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var roleId = 2;
            var user = new User { Id = userId };
            var role = new Role { Id = roleId };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockRoleRepository.Setup(repo => repo.GetByIdAsync(roleId)).ReturnsAsync(role);
            _mockUserRoleRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserRole, bool>>>()))
                .ReturnsAsync(new List<UserRole>()); // No existing role assignment

            // Act
            var result = await _userService.AssignRoleAsync(userId, roleId);

            // Assert
            result.Should().BeTrue();
            _mockUserRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<UserRole>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AssignRoleAsync_WithNonExistingUser_ShouldReturnFalse()
        {
            // Arrange
            var userId = 999;
            var roleId = 2;

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.AssignRoleAsync(userId, roleId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UserExistsAsync_WithExistingUser_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.ExistsAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _userService.UserExistsAsync(userId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UsernameExistsAsync_WithExistingUsername_ShouldReturnTrue()
        {
            // Arrange
            var username = "existinguser";
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { new User { Username = username } });

            // Act
            var result = await _userService.UsernameExistsAsync(username);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task EmailExistsAsync_WithExistingEmail_ShouldReturnTrue()
        {
            // Arrange
            var email = "existing@test.com";
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { new User { Email = email } });

            // Act
            var result = await _userService.EmailExistsAsync(email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPagedResults()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@test.com", FirstName = "John", LastName = "Doe" },
                new User { Id = 2, Username = "user2", Email = "user2@test.com", FirstName = "Jane", LastName = "Smith" },
                new User { Id = 3, Username = "user3", Email = "user3@test.com", FirstName = "Bob", LastName = "Johnson" }
            };

            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetPagedAsync(1, 2, "user");

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(3);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(2);
            result.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task UpdateLastLoginAsync_WithValidUserId_ShouldUpdateLastLogin()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser" };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            await _userService.UpdateLastLoginAsync(userId);

            // Assert
            user.LastLoginDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
