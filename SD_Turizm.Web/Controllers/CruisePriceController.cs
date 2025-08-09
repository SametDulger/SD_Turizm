using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class CruisePriceController : Controller
    {
        private readonly ICruisePriceApiService _cruisePriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly ICruiseApiService _cruiseApiService;

        public CruisePriceController(ICruisePriceApiService cruisePriceApiService, ILookupApiService lookupApiService, ICruiseApiService cruiseApiService)
        {
            _cruisePriceApiService = cruisePriceApiService;
            _lookupApiService = lookupApiService;
            _cruiseApiService = cruiseApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _cruisePriceApiService.GetAllCruisePricesAsync() ?? new List<CruisePriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CruisePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _cruisePriceApiService.CreateCruisePriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Cruise fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _cruisePriceApiService.GetCruisePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _cruisePriceApiService.GetCruisePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CruisePriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _cruisePriceApiService.UpdateCruisePriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Cruise fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _cruisePriceApiService.GetCruisePriceByIdAsync(id);
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
            var result = await _cruisePriceApiService.DeleteCruisePriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Cruise fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Cruises
            var cruises = await _cruiseApiService.GetAllCruisesAsync() ?? new List<CruiseDto>();
            ViewBag.CruiseId = cruises.Select(c => new { Value = c.Id, Text = c.Name }).ToList();

            // Load Room Types
            var roomTypes = await _lookupApiService.GetRoomTypesAsync() ?? new List<dynamic>();
            ViewBag.RoomTypes = roomTypes;

            // Load Board Types
            var boardTypes = await _lookupApiService.GetBoardTypesAsync() ?? new List<dynamic>();
            ViewBag.BoardTypes = boardTypes;

            // Load Room Locations
            var roomLocations = await _lookupApiService.GetRoomLocationsAsync() ?? new List<dynamic>();
            ViewBag.RoomLocations = roomLocations;

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}