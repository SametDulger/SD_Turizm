using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IReportsApiService
    {
        Task<CustomerReportDto?> GetCustomerReportAsync();
        Task<FinancialReportDto?> GetFinancialReportAsync();
        Task<SalesReportDto?> GetSalesReportAsync();
        Task<dynamic?> GetProductReportDataAsync();
        Task<HttpResponseMessage?> ExportCustomerReportAsync(string format = "pdf");
        Task<HttpResponseMessage?> ExportFinancialReportAsync(string format = "pdf");
        Task<HttpResponseMessage?> ExportSalesReportAsync(string format = "pdf");
        Task<HttpResponseMessage?> ExportProductReportAsync(string format = "pdf");
        Task<dynamic?> GetCustomerReportDataAsync();
        Task<dynamic?> GetFinancialReportDataAsync();
        Task<dynamic?> GetSalesReportDataAsync();
    }
}
