using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir.");
                return View();
            }

            try
            {
                var loginRequest = new LoginRequestDto { Username = username, Password = password };
                var loginResponse = await _authApiService.LoginAsync(loginRequest);
                
                if (loginResponse?.Token != null)
                {
                    // Token'ı session'a kaydet
                    HttpContext.Session.SetString("JWTToken", loginResponse.Token);
                    HttpContext.Session.SetString("Username", username);
                    
                    // Cookie authentication için claims oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim("Username", username),
                        new Claim("JWTToken", loginResponse.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = loginResponse.ExpiresAt,
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity), authProperties);
                    
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Giriş hatası: {ex.Message}");
            }
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authApiService.LogoutAsync();
            }
            catch
            {
                // Logout hatası olsa bile session'ı temizle
            }
            
            // Cookie authentication'dan çıkış yap
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Session'ı temizle
            HttpContext.Session.Clear();
            
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 

