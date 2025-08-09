using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILoggingService _loggingService;

        public AuthController(IAuthService authService, ILoggingService loggingService)
        {
            _authService = authService;
            _loggingService = loggingService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request.Username, request.Password);
                if (response == null)
                    return Unauthorized("Invalid credentials");

                _loggingService.LogInformation("User logged in successfully", new { username = request.Username });
                return Ok(response);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error during login", ex, new { username = request.Username });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
                if (response == null)
                    return BadRequest("Registration failed");

                _loggingService.LogInformation("User registered successfully", new { username = request.Username, email = request.Email });
                return Ok(response);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error during registration", ex, new { username = request.Username, email = request.Email });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request.RefreshToken);
                if (response == null)
                    return Unauthorized("Invalid refresh token");

                return Ok(response);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error refreshing token", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("2fa/enable")]
        public async Task<ActionResult<object>> Enable2FA()
        {
            try
            {
                var result = await _authService.Enable2FAAsync();
                return Ok(result);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error enabling 2FA", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("2fa/verify")]
        public async Task<ActionResult<object>> Verify2FA([FromBody] TwoFactorRequestDto request)
        {
            try
            {
                var result = await _authService.Verify2FAAsync(request.Code);
                if (!result)
                    return BadRequest("Invalid 2FA code");

                return Ok(new { success = true });
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error verifying 2FA", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            try
            {
                var roles = await _authService.GetRolesAsync();
                return Ok(roles);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving roles", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
        {
            try
            {
                var result = await _authService.AssignRoleAsync(request.UserId, request.RoleId);
                if (!result)
                    return BadRequest("Failed to assign role");

                _loggingService.LogInformation("Role assigned to user", new { userId = request.UserId, roleId = request.RoleId });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error assigning role", ex, new { userId = request.UserId, roleId = request.RoleId });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpGet("sessions")]
        public async Task<ActionResult<IEnumerable<object>>> GetSessions()
        {
            try
            {
                var sessions = await _authService.GetUserSessionsAsync();
                return Ok(sessions);
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error retrieving user sessions", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpDelete("sessions/{sessionId}")]
        public async Task<IActionResult> RevokeSession(string sessionId)
        {
            try
            {
                var result = await _authService.RevokeSessionAsync(sessionId);
                if (!result)
                    return NotFound("Session not found");

                _loggingService.LogInformation("Session revoked", new { sessionId });
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error revoking session", ex, new { sessionId });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            try
            {
                var result = await _authService.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
                if (!result)
                    return BadRequest("Invalid current password");

                _loggingService.LogInformation("Password changed successfully");
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error changing password", ex);
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(request.Email);
                if (!result)
                    return BadRequest("Email not found");

                _loggingService.LogInformation("Password reset email sent", new { email = request.Email });
                return Ok(new { message = "Password reset email sent" });
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error sending password reset email", ex, new { email = request.Email });
                    return StatusCode(500, "Internal server error");
                }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                _loggingService.LogInformation("User logged out successfully");
                return NoContent();
            }
                            catch (Exception ex)
                {
                    _loggingService.LogError("Error during logout", ex);
                    return StatusCode(500, "Internal server error");
                }
        }
    }
}
