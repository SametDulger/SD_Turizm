using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class RentACarPriceController : Controller
    {
        private readonly IRentACarPriceApiService _rentACarPriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly IRentACarApiService _rentACarApiService;

        public RentACarPriceController(IRentACarPriceApiService rentACarPriceApiService, ILookupApiService lookupApiService, IRentACarApiService rentACarApiService)
        {
            _rentACarPriceApiService = rentACarPriceApiService;
            _lookupApiService = lookupApiService;
            _rentACarApiService = rentACarApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _rentACarPriceApiService.GetAllRentACarPricesAsync() ?? new List<RentACarPriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentACarPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _rentACarPriceApiService.CreateRentACarPriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç kiralama fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _rentACarPriceApiService.GetRentACarPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _rentACarPriceApiService.GetRentACarPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentACarPriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _rentACarPriceApiService.UpdateRentACarPriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç kiralama fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _rentACarPriceApiService.GetRentACarPriceByIdAsync(id);
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
            var result = await _rentACarPriceApiService.DeleteRentACarPriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Araç kiralama fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Rent A Cars
            var rentACars = await _rentACarApiService.GetAllRentACarsAsync() ?? new List<RentACarDto>();
            ViewBag.RentACarId = rentACars.Select(r => new { Value = r.Id, Text = r.Name }).ToList();

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}