using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class ExchangeRateController : Controller
    {
        private readonly IExchangeRateApiService _exchangeRateApiService;
        private readonly ILookupApiService _lookupApiService;

        public ExchangeRateController(IExchangeRateApiService exchangeRateApiService, ILookupApiService lookupApiService)
        {
            _exchangeRateApiService = exchangeRateApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _exchangeRateApiService.GetAllExchangeRatesAsync() ?? new List<ExchangeRateDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExchangeRateDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _exchangeRateApiService.CreateExchangeRateAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Döviz kuru oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _exchangeRateApiService.GetExchangeRateByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _exchangeRateApiService.GetExchangeRateByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExchangeRateDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _exchangeRateApiService.UpdateExchangeRateAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Döviz kuru güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _exchangeRateApiService.GetExchangeRateByIdAsync(id);
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
            var result = await _exchangeRateApiService.DeleteExchangeRateAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Döviz kuru silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}