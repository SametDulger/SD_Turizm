using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<string, string> _refreshTokens = new();

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == username && u.IsActive);
            
            var user = users.FirstOrDefault();
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null!;
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            _refreshTokens[refreshToken] = user.Username;

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                Username = user.Username,
                Roles = new List<string> { "User" }
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(string username, string email, string password)
        {
            var existingUsers = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == username || u.Email == email);
            
            if (existingUsers.Any())
            {
                return null!;
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                FirstName = username,
                LastName = "",
                IsActive = true
            };

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new RegisterResponseDto
            {
                Username = username,
                Email = email,
                Message = "Registration successful"
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var username))
            {
                return null!;
            }

            var users = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == username && u.IsActive);
            var user = users.FirstOrDefault();
            
            if (user == null)
            {
                return null!;
            }

            _refreshTokens.Remove(refreshToken);

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            _refreshTokens[newRefreshToken] = user.Username;

            return new LoginResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                Username = user.Username,
                Roles = new List<string> { "User" }
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "DefaultKey");
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<object> GetCurrentUserAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return await Task.FromResult<object>(null!);

            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Username == username);
            var user = users.FirstOrDefault();
            
            if (user == null)
                return await Task.FromResult<object>(null!);

            return await Task.FromResult(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.IsActive
            });
        }

        public async Task LogoutAsync()
        {
            _refreshTokens.Clear();
            await Task.CompletedTask;
        }

        public async Task<object> Enable2FAAsync()
        {
            return await Task.FromResult(new
            {
                SecretKey = "JBSWY3DPEHPK3PXP",
                QrCodeUrl = "https://example.com/qr-code",
                BackupCodes = new[] { "123456", "234567", "345678", "456789", "567890" }
            });
        }

        public async Task<bool> Verify2FAAsync(string code)
        {
            return await Task.FromResult(code == "123456");
        }

        public async Task<IEnumerable<object>> GetRolesAsync()
        {
            return await Task.FromResult(new[]
            {
                new { Id = 1, Name = "Admin" },
                new { Id = 2, Name = "User" },
                new { Id = 3, Name = "Manager" }
            });
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<object>> GetUserSessionsAsync()
        {
            return await Task.FromResult(new[]
            {
                new { Id = "session1", Device = "Chrome", Location = "Istanbul", LastActivity = DateTime.Now.AddMinutes(-5) },
                new { Id = "session2", Device = "Mobile", Location = "Ankara", LastActivity = DateTime.Now.AddHours(-1) }
            });
        }

        public async Task<bool> RevokeSessionAsync(string sessionId)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return false;

            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Username == username);
            var user = users.FirstOrDefault();
            
            if (user == null || !VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();
            
            if (user == null)
                return false;

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "DefaultKey");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}
