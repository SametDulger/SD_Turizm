using SD_Turizm.Web.Models.DTOs;

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

        public async Task<ProductReportDto?> GetProductReportAsync()
        {
            return await _apiClient.GetAsync<ProductReportDto>("Reports/product");
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
            return await _apiClient.GetAsync<dynamic>("Reports/customer");
        }

        public async Task<dynamic?> GetFinancialReportDataAsync()
        {
            return await _apiClient.GetAsync<dynamic>("Reports/financial");
        }

        public async Task<dynamic?> GetSalesReportDataAsync()
        {
            return await _apiClient.GetAsync<dynamic>("Reports/sales");
        }
    }
}
