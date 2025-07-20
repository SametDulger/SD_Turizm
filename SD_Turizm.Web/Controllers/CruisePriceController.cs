using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD_Turizm.Web.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace SD_Turizm.Web.Controllers
{
    public class CruisePriceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public CruisePriceController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}CruisePrice");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entities = JsonSerializer.Deserialize<List<CruisePriceDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(entities);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }
            return View(new List<CruisePriceDto>());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var cruisesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Cruise");
                if (cruisesResponse.IsSuccessStatusCode)
                {
                    var cruisesContent = await cruisesResponse.Content.ReadAsStringAsync();
                    var cruises = JsonSerializer.Deserialize<List<CruiseDto>>(cruisesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.CruiseId = cruises?.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Code} - {c.Name}" }).ToList();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Kruvaziyer listesi yüklenirken hata oluştu: {ex.Message}");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CruisePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{_apiBaseUrl}CruisePrice", content);
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

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}CruisePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<CruisePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}CruisePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<CruisePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    var cruisesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}Cruise");
                    if (cruisesResponse.IsSuccessStatusCode)
                    {
                        var cruisesContent = await cruisesResponse.Content.ReadAsStringAsync();
                        var cruises = JsonSerializer.Deserialize<List<CruiseDto>>(cruisesContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        ViewBag.CruiseId = cruises?.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Code} - {c.Name}" }).ToList();
                    }
                    
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
        public async Task<IActionResult> Edit(int id, CruisePriceDto entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(entity);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"{_apiBaseUrl}CruisePrice/{id}", content);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}CruisePrice/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var entity = JsonSerializer.Deserialize<CruisePriceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}CruisePrice/{id}");
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
