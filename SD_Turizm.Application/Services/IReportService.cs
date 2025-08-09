using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface IReportService
    {
        Task<IEnumerable<Sale>> GetSalesReportAsync(DateTime startDate, DateTime endDate, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null);
        Task<object> GetSalesSummaryAsync(DateTime startDate, DateTime endDate, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null);
        Task<IEnumerable<Sale>> GetFinancialReportAsync(DateTime startDate, DateTime endDate, string currency = "TRY");
        Task<object> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate, string currency = "TRY");
        Task<IEnumerable<Sale>> GetCustomerReportAsync(DateTime startDate, DateTime endDate, string? cariCode = null);
        Task<object> GetCustomerSummaryAsync(DateTime startDate, DateTime endDate, string? cariCode = null);
        Task<IEnumerable<object>> GetProductReportAsync(DateTime startDate, DateTime endDate, string? productType = null);
        Task<object> GetProductSummaryAsync(DateTime startDate, DateTime endDate, string? productType = null);
        
        // V2 Methods
        Task<PagedResult<object>> GetSalesReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null);
        Task<byte[]> ExportSalesReportAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null);
        Task<PagedResult<object>> GetFinancialReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string currency = "TRY");
        Task<byte[]> ExportFinancialReportAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string currency = "TRY");
        Task<PagedResult<object>> GetCustomerReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? cariCode = null);
        Task<PagedResult<object>> GetProductReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? productType = null);
        Task<object> GetLiveSalesDataAsync();
        Task<object> GetDashboardWidgetsAsync();
        Task<object> GetSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
} 
