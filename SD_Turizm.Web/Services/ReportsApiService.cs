using SD_Turizm.Web.Models.DTOs;
using System.Linq;

namespace SD_Turizm.Web.Services
{
    public class ReportsApiService : IReportsApiService
    {
        private readonly IApiClientService _apiClient;

        public ReportsApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CustomerReportDto?> GetCustomerReportAsync()
        {
            return await _apiClient.GetAsync<CustomerReportDto>("Reports/customer");
        }

        public async Task<FinancialReportDto?> GetFinancialReportAsync()
        {
            return await _apiClient.GetAsync<FinancialReportDto>("Reports/financial");
        }

        public async Task<SalesReportDto?> GetSalesReportAsync()
        {
            return await _apiClient.GetAsync<SalesReportDto>("Reports/sales");
        }

        public async Task<dynamic?> GetProductReportDataAsync()
        {
            var result = await _apiClient.GetAsync<PagedResult<ProductReportDto>>("Reports/product");
            return new { 
                Products = result?.Items ?? new List<ProductReportDto>(),
                Summary = result?.Items?.FirstOrDefault() ?? new ProductReportDto()
            };
        }

        public async Task<HttpResponseMessage?> ExportCustomerReportAsync(string format = "pdf")
        {
            return await _apiClient.GetResponseAsync($"Reports/export/customer?format={format}");
        }

        public async Task<HttpResponseMessage?> ExportFinancialReportAsync(string format = "pdf")
        {
            return await _apiClient.GetResponseAsync($"Reports/export/financial?format={format}");
        }

        public async Task<HttpResponseMessage?> ExportSalesReportAsync(string format = "pdf")
        {
            return await _apiClient.GetResponseAsync($"Reports/export/sales?format={format}");
        }

        public async Task<HttpResponseMessage?> ExportProductReportAsync(string format = "pdf")
        {
            return await _apiClient.GetResponseAsync($"Reports/export/product?format={format}");
        }

        public async Task<dynamic?> GetCustomerReportDataAsync()
        {
            var result = await _apiClient.GetAsync<PagedResult<CustomerReportDto>>("Reports/customer");
            return new { 
                Customers = result?.Items ?? new List<CustomerReportDto>(),
                Summary = result?.Items?.FirstOrDefault() ?? new CustomerReportDto()
            };
        }

        public async Task<dynamic?> GetFinancialReportDataAsync()
        {
            var result = await _apiClient.GetAsync<PagedResult<FinancialReportDto>>("Reports/financial");
            return new { 
                Financial = result?.Items ?? new List<FinancialReportDto>(),
                Summary = result?.Items?.FirstOrDefault() ?? new FinancialReportDto()
            };
        }

        public async Task<dynamic?> GetSalesReportDataAsync()
        {
            var result = await _apiClient.GetAsync<PagedResult<SalesReportDto>>("Reports/sales");
            return new { 
                Sales = result?.Items ?? new List<SalesReportDto>(),
                Summary = result?.Items?.FirstOrDefault() ?? new SalesReportDto()
            };
        }
    }
}
