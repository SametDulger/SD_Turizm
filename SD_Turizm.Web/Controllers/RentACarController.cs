using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class RentACarController : Controller
    {
        private readonly IRentACarApiService _rentACarApiService;

        public RentACarController(IRentACarApiService rentACarApiService)
        {
            _rentACarApiService = rentACarApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _rentACarApiService.GetAllRentACarsAsync();
            return View(entities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentACarDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _rentACarApiService.CreateRentACarAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç kiralama oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _rentACarApiService.GetRentACarByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _rentACarApiService.GetRentACarByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentACarDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _rentACarApiService.UpdateRentACarAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Araç kiralama güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _rentACarApiService.GetRentACarByIdAsync(id);
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
            var result = await _rentACarApiService.DeleteRentACarAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Araç kiralama silinirken hata oluştu.");
            return View();
        }
    }
}