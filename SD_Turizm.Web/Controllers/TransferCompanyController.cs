using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Web.Models.DTOs;
using SD_Turizm.Web.Services;

namespace SD_Turizm.Web.Controllers
{
    [Authorize]
    public class TransferCompanyController : Controller
    {
        private readonly ITransferCompanyApiService _transferCompanyApiService;

        public TransferCompanyController(ITransferCompanyApiService transferCompanyApiService)
        {
            _transferCompanyApiService = transferCompanyApiService;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _transferCompanyApiService.GetAllTransferCompaniesAsync();
            return View(entities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferCompanyDto entity)
        {
            if (ModelState.IsValid)
            {
                var result = await _transferCompanyApiService.CreateTransferCompanyAsync(entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Transfer şirketi oluşturulurken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = await _transferCompanyApiService.GetTransferCompanyByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _transferCompanyApiService.GetTransferCompanyByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransferCompanyDto entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _transferCompanyApiService.UpdateTransferCompanyAsync(id, entity);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Transfer şirketi güncellenirken hata oluştu.");
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _transferCompanyApiService.GetTransferCompanyByIdAsync(id);
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
            var result = await _transferCompanyApiService.DeleteTransferCompanyAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Transfer şirketi silinirken hata oluştu.");
            return View();
        }
    }
}