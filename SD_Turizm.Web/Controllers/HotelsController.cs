using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class HotelsController : Controller
    {
        private readonly IHotelApiService _hotelApiService;
        private readonly ILookupApiService _lookupApiService;

        public HotelsController(IHotelApiService hotelApiService, ILookupApiService lookupApiService)
        {
            _hotelApiService = hotelApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var hotels = await _hotelApiService.GetAllHotelsAsync();
            await LoadLookupData();
            return View(hotels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _hotelApiService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelDto hotel)
        {
            if (ModelState.IsValid)
            {
                var result = await _hotelApiService.CreateHotelAsync(hotel);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating hotel");
            }
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _hotelApiService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelDto hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _hotelApiService.UpdateHotelAsync(id, hotel);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating hotel");
            }
            return View(hotel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _hotelApiService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _hotelApiService.DeleteHotelAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error deleting hotel");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Basic lookup data if needed for hotel forms
        }
    }
} 
