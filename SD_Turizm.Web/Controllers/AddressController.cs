using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressApiService _addressApiService;
        private readonly ILookupApiService _lookupApiService;

        public AddressController(IAddressApiService addressApiService, ILookupApiService lookupApiService)
        {
            _addressApiService = addressApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _addressApiService.GetAllAddressesAsync() ?? new List<AddressDto>();
            LoadLookupData();
            return View(entities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _addressApiService.CreateAddressAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Adres oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _addressApiService.GetAddressByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _addressApiService.GetAddressByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddressDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _addressApiService.UpdateAddressAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Adres güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _addressApiService.GetAddressByIdAsync(id);
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
            var result = await _addressApiService.DeleteAddressAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Adres silinirken hata oluştu.");
            return View();
        }

        private void LoadLookupData()
        {
            // Basic lookup data if needed for address forms
            // Currently no specific lookup data needed for addresses
        }
    }
}