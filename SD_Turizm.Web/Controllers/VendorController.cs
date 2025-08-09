using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private readonly IVendorApiService _vendorApiService;
        private readonly ILookupApiService _lookupApiService;

        public VendorController(IVendorApiService vendorApiService, ILookupApiService lookupApiService)
        {
            _vendorApiService = vendorApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var entities = await _vendorApiService.GetAllVendorsAsync() ?? new List<VendorDto>();
                await LoadLookupData();
                return View(entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<VendorDto>());
            }
        }

        public async Task<IActionResult> ActiveVendors()
        {
            try
            {
                var entities = await _vendorApiService.GetActiveVendorsAsync() ?? new List<VendorDto>();
                return View(entities);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(new List<VendorDto>());
            }
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookupData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendorDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _vendorApiService.CreateVendorAsync(entity);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Tedarikçi oluşturulurken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var entity = await _vendorApiService.GetVendorByIdAsync(id);
                if (entity != null)
                {
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var entity = await _vendorApiService.GetVendorByIdAsync(id);
                if (entity != null)
                {
                    await LoadLookupData();
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VendorDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _vendorApiService.UpdateVendorAsync(id, entity);
                    if (result != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Tedarikçi güncellenirken hata oluştu.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _vendorApiService.GetVendorByIdAsync(id);
                if (entity != null)
                {
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _vendorApiService.DeleteVendorAsync(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Tedarikçi silinirken hata oluştu.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookupData()
        {
            var vendorTypes = await _lookupApiService.GetVendorTypesAsync() ?? new List<dynamic>();
            ViewBag.VendorTypes = vendorTypes;
        }
    }
}