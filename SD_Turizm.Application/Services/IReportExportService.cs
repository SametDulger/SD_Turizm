using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IReportExportService
    {
        Task<byte[]> ExportToExcelAsync(object data, string sheetName = "Rapor");
        Task<byte[]> ExportToPdfAsync(object data, string title = "Rapor");
        Task<byte[]> ExportToCsvAsync(object data, string fileName = "rapor");
        Task<string> ExportToJsonAsync(object data);
        Task<byte[]> ExportSalesReportAsync(DateTime startDate, DateTime endDate, string format = "excel");
        Task<byte[]> ExportCustomerReportAsync(DateTime startDate, DateTime endDate, string format = "excel");
        Task<byte[]> ExportFinancialReportAsync(DateTime startDate, DateTime endDate, string format = "excel");
        Task<byte[]> ExportProductReportAsync(DateTime startDate, DateTime endDate, string format = "excel");
        Task<byte[]> ExportAuditReportAsync(DateTime startDate, DateTime endDate, string format = "excel");
        Task<byte[]> ExportCustomReportAsync(object data, string reportType, string format = "excel");
    }
}
