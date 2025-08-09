using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class TourPriceController : Controller
    {
        private readonly ITourPriceApiService _tourPriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly ITourApiService _tourApiService;

        public TourPriceController(ITourPriceApiService tourPriceApiService, ILookupApiService lookupApiService, ITourApiService tourApiService)
        {
            _tourPriceApiService = tourPriceApiService;
            _lookupApiService = lookupApiService;
            _tourApiService = tourApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _tourPriceApiService.GetAllTourPricesAsync() ?? new List<TourPriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourPriceApiService.CreateTourPriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Tur fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _tourPriceApiService.GetTourPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _tourPriceApiService.GetTourPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourPriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _tourPriceApiService.UpdateTourPriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Tur fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _tourPriceApiService.GetTourPriceByIdAsync(id);
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
            var result = await _tourPriceApiService.DeleteTourPriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Tur fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Tours
            var tours = await _tourApiService.GetAllToursAsync() ?? new List<TourDto>();
            ViewBag.TourId = tours.Select(t => new { Value = t.Id, Text = t.Name }).ToList();

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}