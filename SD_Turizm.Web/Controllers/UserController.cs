using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiService _userApiService;
        private readonly ILookupApiService _lookupApiService;

        public UserController(IUserApiService userApiService, ILookupApiService lookupApiService)
        {
            _userApiService = userApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userApiService.GetAllUsersAsync() ?? new List<UserDto>();
                return View(users);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<UserDto>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userApiService.GetUserByIdAsync(id);
                if (user != null)
                {
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userApiService.CreateUserAsync(user);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Kullanıcı oluşturulurken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userApiService.GetUserByIdAsync(id);
                if (user != null)
                {
                    await LoadLookupData();
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userApiService.UpdateUserAsync(id, user);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Kullanıcı güncellenirken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userApiService.GetUserByIdAsync(id);
                if (user != null)
                {
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _userApiService.DeleteUserAsync(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Kullanıcı silinirken hata oluştu.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword(int id)
        {
            try
            {
                var user = await _userApiService.GetUserByIdAsync(id);
                if (user != null)
                {
                    return View(new ChangePasswordDto { UserId = id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userApiService.ChangePasswordAsync(id, model);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi.";
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Şifre değiştirilirken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(model);
        }

        private async Task LoadLookupData()
        {
            var roles = await _lookupApiService.GetRolesAsync() ?? new List<dynamic>();
            ViewBag.Roles = roles;
        }
    }
}
