using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class SaleItemController : Controller
    {
        private readonly ISaleItemApiService _saleItemApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly ISaleApiService _saleApiService;

        public SaleItemController(ISaleItemApiService saleItemApiService, ILookupApiService lookupApiService, ISaleApiService saleApiService)
        {
            _saleItemApiService = saleItemApiService;
            _lookupApiService = lookupApiService;
            _saleApiService = saleApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _saleItemApiService.GetAllSaleItemsAsync() ?? new List<SaleItemDto>();
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
        public async Task<IActionResult> Create(SaleItemDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _saleItemApiService.CreateSaleItemAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Satış öğesi oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _saleItemApiService.GetSaleItemByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _saleItemApiService.GetSaleItemByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleItemDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _saleItemApiService.UpdateSaleItemAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Satış öğesi güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _saleItemApiService.GetSaleItemByIdAsync(id);
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
            var result = await _saleItemApiService.DeleteSaleItemAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Satış öğesi silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Sales
            var sales = await _saleApiService.GetAllSalesAsync() ?? new List<SaleDto>();
            ViewBag.SaleId = sales.Select(s => new { Value = s.Id, Text = s.PNRNumber }).ToList();

            // Load Vendor Types
            var vendorTypes = await _lookupApiService.GetVendorTypesAsync() ?? new List<dynamic>();
            ViewBag.VendorTypes = vendorTypes;

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}