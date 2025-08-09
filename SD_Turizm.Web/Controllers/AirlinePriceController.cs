using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class AirlinePriceController : Controller
    {
        private readonly IAirlinePriceApiService _airlinePriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly IAirlineApiService _airlineApiService;

        public AirlinePriceController(IAirlinePriceApiService airlinePriceApiService, ILookupApiService lookupApiService, IAirlineApiService airlineApiService)
        {
            _airlinePriceApiService = airlinePriceApiService;
            _lookupApiService = lookupApiService;
            _airlineApiService = airlineApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _airlinePriceApiService.GetAllAirlinePricesAsync() ?? new List<AirlinePriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirlinePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _airlinePriceApiService.CreateAirlinePriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Havayolu fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _airlinePriceApiService.GetAirlinePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _airlinePriceApiService.GetAirlinePriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AirlinePriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _airlinePriceApiService.UpdateAirlinePriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Havayolu fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _airlinePriceApiService.GetAirlinePriceByIdAsync(id);
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
            var result = await _airlinePriceApiService.DeleteAirlinePriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Havayolu fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Airlines
            var airlines = await _airlineApiService.GetAllAirlinesAsync() ?? new List<AirlineDto>();
            ViewBag.AirlineId = airlines.Select(a => new { Value = a.Id, Text = a.Name }).ToList();

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}