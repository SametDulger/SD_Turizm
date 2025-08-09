using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly IAuditApiService _auditApiService;
        private readonly ILookupApiService _lookupApiService;

        public AuditController(IAuditApiService auditApiService, ILookupApiService lookupApiService)
        {
            _auditApiService = auditApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var entities = await _auditApiService.GetAllAuditLogsAsync() ?? new List<AuditLogDto>();
                LoadLookupData();
                return View(entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<AuditLogDto>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var entity = await _auditApiService.GetAuditLogByIdAsync(id);
                if (entity != null)
                {
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ByUser(string userId)
        {
            try
            {
                if (int.TryParse(userId, out int userIdInt))
                {
                    var entities = await _auditApiService.GetAuditLogsByUserAsync(userIdInt) ?? new List<AuditLogDto>();
                    return View("Index", entities);
                }
                return View("Index", new List<AuditLogDto>());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View("Index", new List<AuditLogDto>());
            }
        }

        public async Task<IActionResult> ByAction(string action)
        {
            try
            {
                var entities = await _auditApiService.GetAuditLogsByActionAsync(action) ?? new List<AuditLogDto>();
                return View("Index", entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View("Index", new List<AuditLogDto>());
            }
        }

        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var entities = await _auditApiService.GetAuditLogsByDateRangeAsync(startDate, endDate) ?? new List<AuditLogDto>();
                return View("Index", entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View("Index", new List<AuditLogDto>());
            }
        }

        private void LoadLookupData()
        {
            // Basic lookup data for audit forms
        }
    }
}