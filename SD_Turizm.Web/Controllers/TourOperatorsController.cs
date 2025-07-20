using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Controllers
{
    public class TourOperatorsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public TourOperatorsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            var tourOperators = await _httpClient.GetFromJsonAsync<List<TourOperatorDto>>($"{_apiBaseUrl}TourOperators");
            return View(tourOperators);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tourOperator = await _httpClient.GetFromJsonAsync<TourOperatorDto>($"{_apiBaseUrl}TourOperators/{id}");
            if (tourOperator == null)
            {
                return NotFound();
            }
            return View(tourOperator);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourOperatorDto tourOperator)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}TourOperators", tourOperator);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating tour operator");
            }
            return View(tourOperator);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tourOperator = await _httpClient.GetFromJsonAsync<TourOperatorDto>($"{_apiBaseUrl}TourOperators/{id}");
            if (tourOperator == null)
            {
                return NotFound();
            }
            return View(tourOperator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourOperatorDto tourOperator)
        {
            if (id != tourOperator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}TourOperators/{id}", tourOperator);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating tour operator");
            }
            return View(tourOperator);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tourOperator = await _httpClient.GetFromJsonAsync<TourOperatorDto>($"{_apiBaseUrl}TourOperators/{id}");
            if (tourOperator == null)
            {
                return NotFound();
            }
            return View(tourOperator);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}TourOperators/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
