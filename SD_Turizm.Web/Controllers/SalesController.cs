using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Controllers
{
    public class SalesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SalesController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:7000/api/";
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _httpClient.GetFromJsonAsync<List<SaleDto>>($"{_apiBaseUrl}Sales");
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<SaleDto>($"{_apiBaseUrl}Sales/{id}");
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        public async Task<IActionResult> DetailsByPNR(string pnrNumber)
        {
            var sale = await _httpClient.GetFromJsonAsync<SaleDto>($"{_apiBaseUrl}Sales/pnr/{pnrNumber}");
            if (sale == null)
            {
                return NotFound();
            }
            return View("Details", sale);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleDto sale)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}Sales", sale);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating sale");
            }
            return View(sale);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<SaleDto>($"{_apiBaseUrl}Sales/{id}");
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleDto sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}Sales/{id}", sale);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating sale");
            }
            return View(sale);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<SaleDto>($"{_apiBaseUrl}Sales/{id}");
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}Sales/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var sales = await _httpClient.GetFromJsonAsync<List<SaleDto>>($"{_apiBaseUrl}Sales/date-range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return View("Index", sales);
        }

        public async Task<IActionResult> ByAgency(string agencyCode)
        {
            var sales = await _httpClient.GetFromJsonAsync<List<SaleDto>>($"{_apiBaseUrl}Sales/agency/{agencyCode}");
            return View("Index", sales);
        }

        public async Task<IActionResult> ByCariCode(string cariCode)
        {
            var sales = await _httpClient.GetFromJsonAsync<List<SaleDto>>($"{_apiBaseUrl}Sales/cari/{cariCode}");
            return View("Index", sales);
        }
    }
} 
