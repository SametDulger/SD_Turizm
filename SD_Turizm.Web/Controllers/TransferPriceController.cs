using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class TransferPriceController : Controller
    {
        private readonly ITransferPriceApiService _transferPriceApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly ITransferCompanyApiService _transferCompanyApiService;

        public TransferPriceController(ITransferPriceApiService transferPriceApiService, ILookupApiService lookupApiService, ITransferCompanyApiService transferCompanyApiService)
        {
            _transferPriceApiService = transferPriceApiService;
            _lookupApiService = lookupApiService;
            _transferCompanyApiService = transferCompanyApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _transferPriceApiService.GetAllTransferPricesAsync() ?? new List<TransferPriceDto>();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _transferPriceApiService.CreateTransferPriceAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Transfer fiyatı oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _transferPriceApiService.GetTransferPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _transferPriceApiService.GetTransferPriceByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransferPriceDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _transferPriceApiService.UpdateTransferPriceAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Transfer fiyatı güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _transferPriceApiService.GetTransferPriceByIdAsync(id);
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
            var result = await _transferPriceApiService.DeleteTransferPriceAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Transfer fiyatı silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Transfer Companies
            var transferCompanies = await _transferCompanyApiService.GetAllTransferCompaniesAsync() ?? new List<TransferCompanyDto>();
            ViewBag.TransferCompanyId = transferCompanies.Select(tc => new { Value = tc.Id, Text = tc.Name }).ToList();

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}