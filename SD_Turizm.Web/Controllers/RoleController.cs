using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleApiService _roleApiService;
        private readonly ILookupApiService _lookupApiService;

        public RoleController(IRoleApiService roleApiService, ILookupApiService lookupApiService)
        {
            _roleApiService = roleApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _roleApiService.GetAllRolesAsync() ?? new List<RoleDto>();
                await LoadLookupData();
                return View(roles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<RoleDto>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var role = await _roleApiService.GetRoleByIdAsync(id);
                if (role != null)
                {
                    return View(role);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDto role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _roleApiService.CreateRoleAsync(role);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Rol oluşturulurken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var role = await _roleApiService.GetRoleByIdAsync(id);
                if (role != null)
                {
                    return View(role);
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
        public async Task<IActionResult> Edit(int id, RoleDto role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _roleApiService.UpdateRoleAsync(id, role);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Rol güncellenirken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(role);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = await _roleApiService.GetRoleByIdAsync(id);
                if (role != null)
                {
                    return View(role);
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
                var result = await _roleApiService.DeleteRoleAsync(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Rol silinirken hata oluştu.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Permissions(int id)
        {
            try
            {
                var permissions = await _roleApiService.GetRolePermissionsAsync(id);
                if (permissions != null)
                {
                    ViewBag.RoleId = id;
                    return View(permissions);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        private async Task LoadLookupData()
        {
            // Basic lookup data if needed for role forms
        }
    }


}
