using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class HotelPriceController : Controller
    {
        private readonly IHotelPriceApiService _hotelPriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly IHotelApiService _hotelApiService;

        public HotelPriceController(IHotelPriceApiService hotelPriceApiService, ILookupApiService lookupApiService, IHotelApiService hotelApiService)
        {
            _hotelPriceApiService = hotelPriceApiService;
            _lookupApiService = lookupApiService;
            _hotelApiService = hotelApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _hotelPriceApiService.GetAllHotelPricesAsync() ?? new List<HotelPriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _hotelPriceApiService.CreateHotelPriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Otel fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _hotelPriceApiService.GetHotelPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _hotelPriceApiService.GetHotelPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelPriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _hotelPriceApiService.UpdateHotelPriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Otel fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _hotelPriceApiService.GetHotelPriceByIdAsync(id);
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
            var result = await _hotelPriceApiService.DeleteHotelPriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Otel fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Hotels
            var hotels = await _hotelApiService.GetAllHotelsAsync() ?? new List<HotelDto>();
            ViewBag.HotelId = hotels.Select(h => new { Value = h.Id, Text = h.Name }).ToList();

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