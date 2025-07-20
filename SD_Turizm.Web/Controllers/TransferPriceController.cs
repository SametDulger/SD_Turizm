using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD_Turizm.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    public class TransferPriceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public TransferPriceController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}TransferPrice");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonSerializer.Deserialize<List<TransferPriceDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entities);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(new List<TransferPriceDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}TransferPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<TransferPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var transferCompaniesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}TransferCompany");
                if (transferCompaniesResponse.IsSuccessStatusCode)
                {
                    var transferCompaniesContent = await transferCompaniesResponse.Content.ReadAsStringAsync();
                    var transferCompanies = JsonSerializer.Deserialize<List<TransferCompanyDto>>(transferCompaniesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.TransferCompanyId = transferCompanies?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = $"{t.Code} - {t.CompanyName}" }).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Transfer şirketi listesi yüklenirken hata oluştu: {ex.Message}");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferPriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}TransferPrice", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}TransferPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<TransferPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var transferCompaniesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}TransferCompany");
                    if (transferCompaniesResponse.IsSuccessStatusCode)
                    {
                        var transferCompaniesContent = await transferCompaniesResponse.Content.ReadAsStringAsync();
                        var transferCompanies = JsonSerializer.Deserialize<List<TransferCompanyDto>>(transferCompaniesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewBag.TransferCompanyId = transferCompanies?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = $"{t.Code} - {t.CompanyName}" }).ToList();
                    }
                    
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
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
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{_apiBaseUrl}TransferPrice/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}TransferPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<TransferPriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}TransferPrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
