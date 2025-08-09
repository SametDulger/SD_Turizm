using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class SalePersonController : Controller
    {
        private readonly ISalePersonApiService _salePersonApiService;
        private readonly ILookupApiService _lookupApiService;

        public SalePersonController(ISalePersonApiService salePersonApiService, ILookupApiService lookupApiService)
        {
            _salePersonApiService = salePersonApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _salePersonApiService.GetAllSalePersonsAsync() ?? new List<SalePersonDto>();
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
        public async Task<IActionResult> Create(SalePersonDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _salePersonApiService.CreateSalePersonAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Satış personeli oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _salePersonApiService.GetSalePersonByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _salePersonApiService.GetSalePersonByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            LoadLookupData();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalePersonDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _salePersonApiService.UpdateSalePersonAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Satış personeli güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _salePersonApiService.GetSalePersonByIdAsync(id);
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
            var result = await _salePersonApiService.DeleteSalePersonAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Satış personeli silinirken hata oluştu.");
            return View();
        }

        private async Task LoadLookupData()
        {
            var personTypes = await _lookupApiService.GetPersonTypesAsync() ?? new List<dynamic>();
            ViewBag.PersonTypes = personTypes;
        }
    }
}