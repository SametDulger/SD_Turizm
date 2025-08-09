using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class CariTransactionController : Controller
    {
        private readonly ICariTransactionApiService _cariTransactionApiService;
        private readonly ILookupApiService _lookupApiService;

        public CariTransactionController(ICariTransactionApiService cariTransactionApiService, ILookupApiService lookupApiService)
        {
            _cariTransactionApiService = cariTransactionApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _cariTransactionApiService.GetAllTransactionsAsync() ?? new List<CariTransactionDto>();
            await LoadLookupData();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CariTransactionDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _cariTransactionApiService.CreateTransactionAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Cari işlem oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _cariTransactionApiService.GetTransactionByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _cariTransactionApiService.GetTransactionByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CariTransactionDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _cariTransactionApiService.UpdateTransactionAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Cari işlem güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _cariTransactionApiService.GetTransactionByIdAsync(id);
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
            var result = await _cariTransactionApiService.DeleteTransactionAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Cari işlem silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;

            var transactionTypes = await _lookupApiService.GetTransactionTypesAsync() ?? new List<dynamic>();
            ViewBag.TransactionTypes = transactionTypes;
        }
    }
}