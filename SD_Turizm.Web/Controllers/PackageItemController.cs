using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class PackageItemController : Controller
    {
        private readonly IPackageItemApiService _packageItemApiService;
        private readonly ILookupApiService _lookupApiService;
        private readonly IPackageApiService _packageApiService;

        public PackageItemController(IPackageItemApiService packageItemApiService, ILookupApiService lookupApiService, IPackageApiService packageApiService)
        {
            _packageItemApiService = packageItemApiService;
            _lookupApiService = lookupApiService;
            _packageApiService = packageApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _packageItemApiService.GetAllPackageItemsAsync() ?? new List<PackageItemDto>();
            LoadLookupData();
            return View(entities);
        }

        public async Task<IActionResult> Create()
        {
            LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackageItemDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _packageItemApiService.CreatePackageItemAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Paket öğesi oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _packageItemApiService.GetPackageItemByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _packageItemApiService.GetPackageItemByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PackageItemDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _packageItemApiService.UpdatePackageItemAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Paket öğesi güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _packageItemApiService.GetPackageItemByIdAsync(id);
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
            var result = await _packageItemApiService.DeletePackageItemAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Paket öğesi silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            // Load Packages
            var packages = await _packageApiService.GetAllPackagesAsync() ?? new List<PackageDto>();
            ViewBag.PackageId = packages.Select(p => new { Value = p.Id, Text = p.Name }).ToList();

            // Load Vendor Types
            var vendorTypes = await _lookupApiService.GetVendorTypesAsync() ?? new List<dynamic>();
            ViewBag.VendorTypes = vendorTypes;

            // Load Currencies
            var currencies = await _lookupApiService.GetCurrenciesAsync() ?? new List<dynamic>();
            ViewBag.Currencies = currencies;
        }
    }
}