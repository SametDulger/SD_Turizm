using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Başarıyla çıkış yapıldı" });
        }

        [HttpGet("validate")]
        public async Task<ActionResult<bool>> ValidateToken([FromHeader(Name = "Authorization")] string authorization)
        {
            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Geçersiz token formatı" });
            }

            var token = authorization.Substring("Bearer ".Length);
            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(isValid);
        }
    }
} 