using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggingService _loggingService;

        public UserController(IUserService userService, ILoggingService loggingService)
        {
            _userService = userService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all users", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting user with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            try
            {
                var user = await _userService.GetByUsernameAsync(username);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting user with username {username}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting user with email {email}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                };

                var createdUser = await _userService.CreateAsync(user, request.Password);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating user", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var existingUser = await _userService.GetByIdAsync(id);
                if (existingUser == null)
                    return NotFound();

                existingUser.Username = request.Username;
                existingUser.Email = request.Email;
                existingUser.FirstName = request.FirstName;
                existingUser.LastName = request.LastName;
                existingUser.PhoneNumber = request.PhoneNumber;

                var updatedUser = await _userService.UpdateAsync(existingUser);
                return Ok(updatedUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating user with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (!await _userService.UserExistsAsync(id))
                    return NotFound();

                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting user with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/change-password")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            try
            {
                var success = await _userService.ChangePasswordAsync(id, request.CurrentPassword, request.NewPassword);
                if (!success)
                    return BadRequest("Mevcut şifre yanlış");

                return Ok(new { message = "Şifre başarıyla değiştirildi" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error changing password for user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var success = await _userService.ResetPasswordAsync(request.Email);
                if (!success)
                    return BadRequest("E-posta adresi bulunamadı");

                return Ok(new { message = "Şifre sıfırlama e-postası gönderildi" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error resetting password for email {request.Email}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<ActionResult> Activate(int id)
        {
            try
            {
                var success = await _userService.ActivateUserAsync(id);
                if (!success)
                    return NotFound();

                return Ok(new { message = "Kullanıcı aktifleştirildi" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error activating user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult> Deactivate(int id)
        {
            try
            {
                var success = await _userService.DeactivateUserAsync(id);
                if (!success)
                    return NotFound();

                return Ok(new { message = "Kullanıcı deaktifleştirildi" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deactivating user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetUserRoles(int id)
        {
            try
            {
                var roles = await _userService.GetUserRolesAsync(id);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting roles for user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/roles/{roleId}")]
        public async Task<ActionResult> AssignRole(int id, int roleId)
        {
            try
            {
                var success = await _userService.AssignRoleAsync(id, roleId);
                if (!success)
                    return BadRequest("Kullanıcı veya rol bulunamadı");

                return Ok(new { message = "Rol başarıyla atandı" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error assigning role {roleId} to user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}/roles/{roleId}")]
        public async Task<ActionResult> RemoveRole(int id, int roleId)
        {
            try
            {
                var success = await _userService.RemoveRoleAsync(id, roleId);
                if (!success)
                    return BadRequest("Rol ataması bulunamadı");

                return Ok(new { message = "Rol başarıyla kaldırıldı" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing role {roleId} from user {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<User>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            try
            {
                var result = await _userService.GetPagedAsync(page, pageSize, searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting paged users", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
