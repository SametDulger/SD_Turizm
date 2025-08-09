using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class TourController : Controller
    {
        private readonly ITourApiService _tourApiService;
        private readonly ILookupApiService _lookupApiService;

        public TourController(ITourApiService tourApiService, ILookupApiService lookupApiService)
        {
            _tourApiService = tourApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var entities = await _tourApiService.GetAllToursAsync();
                await LoadLookupData();
                return View(entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<TourDto>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _tourApiService.CreateTourAsync(entity);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var entity = await _tourApiService.GetTourByIdAsync(id);
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var entity = await _tourApiService.GetTourByIdAsync(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _tourApiService.UpdateTourAsync(id, entity);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _tourApiService.GetTourByIdAsync(id);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _tourApiService.DeleteTourAsync(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Delete), new { id });
        }

        private async Task LoadLookupData()
        {
            // Basic lookup data if needed for tour forms
        }
    }
} 
