using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleApiService _saleApiService;
        private readonly ILookupApiService _lookupApiService;

        public SalesController(ISaleApiService saleApiService, ILookupApiService lookupApiService)
        {
            _saleApiService = saleApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _saleApiService.GetAllSalesAsync();
            await LoadLookupData();
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _saleApiService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        public IActionResult DetailsByPNR(string pnrNumber)
        {
            // Bu endpoint SaleApiService'de yok, şimdilik Details'e yönlendir
            return RedirectToAction(nameof(Details), new { id = 0 });
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleDto sale)
        {
            if (ModelState.IsValid)
            {
                var result = await _saleApiService.CreateSaleAsync(sale);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating sale");
            }
            return View(sale);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sale = await _saleApiService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            await LoadLookupData();
            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleDto sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _saleApiService.UpdateSaleAsync(id, sale);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating sale");
            }
            return View(sale);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _saleApiService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _saleApiService.DeleteSaleAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error deleting sale");
            return View();
        }

        public IActionResult ByDateRange(DateTime startDate, DateTime endDate)
        {
            // Bu endpoint SaleApiService'de yok, şimdilik Index'e yönlendir
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ByAgency(string agencyCode)
        {
            // Bu endpoint SaleApiService'de yok, şimdilik Index'e yönlendir
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ByCariCode(string cariCode)
        {
            // Bu endpoint SaleApiService'de yok, şimdilik Index'e yönlendir
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupData()
        {
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;

            var saleStatuses = await _lookupApiService.GetSaleStatusesAsync() ?? new List<dynamic>();
            ViewBag.SaleStatuses = saleStatuses;
        }
    }
} 
