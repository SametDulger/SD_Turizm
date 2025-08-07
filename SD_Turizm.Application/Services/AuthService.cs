using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SD_Turizm.Core.DTOs;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, string> _refreshTokens = new();

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == loginDto.Username && u.IsActive);
            
            var user = users.FirstOrDefault();
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı adı veya şifre");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            _refreshTokens[refreshToken] = user.Username;

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = await GetUserDtoAsync(user)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUsers = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == registerDto.Username || u.Email == registerDto.Email);
            
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("Kullanıcı adı veya email zaten kullanımda");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true
            };

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            _refreshTokens[refreshToken] = user.Username;

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = await GetUserDtoAsync(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var username))
            {
                throw new UnauthorizedAccessException("Geçersiz refresh token");
            }

            var users = await _unitOfWork.Repository<User>().FindAsync(u => 
                u.Username == username && u.IsActive);
            var user = users.FirstOrDefault();
            
            if (user == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            
            _refreshTokens.Remove(refreshToken);
            _refreshTokens[newRefreshToken] = user.Username;

            return new AuthResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = await GetUserDtoAsync(user)
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
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            // Bu metod JWT token'dan user bilgilerini çıkaracak
            // Şimdilik basit bir implementasyon
            throw new NotImplementedException("GetCurrentUserAsync metodu JWT middleware ile implement edilecek");
        }

        public async Task LogoutAsync()
        {
            // Refresh token'ları temizleme işlemi
            // Şimdilik basit bir implementasyon
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "DefaultKey");
            
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
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
            return HashPassword(password) == hash;
        }

        private async Task<UserDto> GetUserDtoAsync(User user)
        {
            var userRoles = await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == user.Id);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
            
            var roles = await _unitOfWork.Repository<Role>().FindAsync(r => roleIds.Contains(r.Id));
            var roleNames = roles.Select(r => r.Name).ToList();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roleNames,
                Permissions = new List<string>() // Şimdilik boş, daha sonra permission'lar eklenecek
            };
        }
    }
} 