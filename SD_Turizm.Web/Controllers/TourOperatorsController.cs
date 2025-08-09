using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class TourOperatorsController : Controller
    {
        private readonly ITourOperatorApiService _tourOperatorApiService;
        private readonly ILookupApiService _lookupApiService;

        public TourOperatorsController(ITourOperatorApiService tourOperatorApiService, ILookupApiService lookupApiService)
        {
            _tourOperatorApiService = tourOperatorApiService;
            _lookupApiService = lookupApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _tourOperatorApiService.GetAllTourOperatorsAsync() ?? new List<TourOperatorDto>();
            LoadLookupData();
            return View(entities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourOperatorDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourOperatorApiService.CreateTourOperatorAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Tur operatörü oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _tourOperatorApiService.GetTourOperatorByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _tourOperatorApiService.GetTourOperatorByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourOperatorDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _tourOperatorApiService.UpdateTourOperatorAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Tur operatörü güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _tourOperatorApiService.GetTourOperatorByIdAsync(id);
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
            var result = await _tourOperatorApiService.DeleteTourOperatorAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Tur operatörü silinirken hata oluştu.");
            return View();
        }

        private void LoadLookupData()
        {
            // Basic lookup data if needed for tour operator forms
        }
    }
}