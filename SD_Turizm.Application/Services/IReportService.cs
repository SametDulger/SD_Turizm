using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

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
    }
} 