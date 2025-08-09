using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class GuidePriceController : Controller
    {
        private readonly IGuidePriceApiService _guidePriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly IGuideApiService _guideApiService;

        public GuidePriceController(IGuidePriceApiService guidePriceApiService, ILookupApiService lookupApiService, IGuideApiService guideApiService)
        {
            _guidePriceApiService = guidePriceApiService;
            _lookupApiService = lookupApiService;
            _guideApiService = guideApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _guidePriceApiService.GetAllGuidePricesAsync() ?? new List<GuidePriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuidePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _guidePriceApiService.CreateGuidePriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Rehber fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _guidePriceApiService.GetGuidePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _guidePriceApiService.GetGuidePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GuidePriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _guidePriceApiService.UpdateGuidePriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Rehber fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _guidePriceApiService.GetGuidePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _guidePriceApiService.DeleteGuidePriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Rehber fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Guides
            var guides = await _guideApiService.GetAllGuidesAsync() ?? new List<GuideDto>();
            ViewBag.GuideId = guides.Select(g => new { Value = g.Id, Text = g.Name }).ToList();

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}