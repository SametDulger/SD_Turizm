using System.Security.Cryptography;
using System.Text;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.Repository<User>().GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<User>().GetByIdAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Username == username);
            return users.FirstOrDefault();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == email);
            return users.FirstOrDefault();
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            // Check if username or email already exists
            if (await UsernameExistsAsync(user.Username))
                throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");

            if (await EmailExistsAsync(user.Email))
                throw new InvalidOperationException("Bu e-posta adresi zaten kullanılıyor.");

            // Hash password
            user.PasswordHash = HashPassword(password);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("Kullanıcı bulunamadı.");

            // Check if username or email is changed and already exists
            if (user.Username != existingUser.Username && await UsernameExistsAsync(user.Username))
                throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");

            if (user.Email != existingUser.Email && await EmailExistsAsync(user.Email))
                throw new InvalidOperationException("Bu e-posta adresi zaten kullanılıyor.");

            user.UpdatedDate = DateTime.UtcNow;
            user.PasswordHash = existingUser.PasswordHash; // Don't change password here

            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<User>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            if (user == null) return false;

            // Generate temporary password
            var tempPassword = GenerateTemporaryPassword();
            user.PasswordHash = HashPassword(tempPassword);
            user.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Send email with temporary password
            // In production, implement email service integration
            await Task.Delay(100); // Simulate email sending
            return true;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            user.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
        {
            var userRoles = await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == userId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
            
            if (!roleIds.Any()) return new List<Role>();

            var roles = await _unitOfWork.Repository<Role>().FindAsync(r => roleIds.Contains(r.Id));
            return roles;
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            // Check if user and role exist
            var user = await GetByIdAsync(userId);
            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(roleId);

            if (user == null || role == null) return false;

            // Check if role is already assigned
            var existingUserRole = await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (existingUserRole.Any()) return true; // Already assigned

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRoles = await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            var userRole = userRoles.FirstOrDefault();

            if (userRole == null) return false;

            await _unitOfWork.Repository<UserRole>().DeleteAsync(userRole.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _unitOfWork.Repository<User>().ExistsAsync(id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Username == username);
            return users.Any();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var users = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == email);
            return users.Any();
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            var query = _unitOfWork.Repository<User>().GetAllAsync().Result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => 
                    u.Username.Contains(searchTerm) || 
                    u.Email.Contains(searchTerm) || 
                    u.FirstName.Contains(searchTerm) || 
                    u.LastName.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLoginDate = DateTime.UtcNow;
                await _unitOfWork.Repository<User>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
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

        private string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
