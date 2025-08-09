using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale?> GetSaleByIdAsync(int id);
        Task<Sale?> GetSaleByPNRAsync(string pnrNumber);
        Task<Sale> CreateSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task DeleteSaleAsync(int id);
        Task<bool> SaleExistsAsync(int id);
        Task<bool> PNRExistsAsync(string pnrNumber);
        Task<string> GeneratePNRNumberAsync();
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Sale>> GetSalesByAgencyAsync(string agencyCode);
        Task<IEnumerable<Sale>> GetSalesByCariCodeAsync(string cariCode);
        
        // V2 Methods
        Task<PagedResult<Sale>> GetSalesWithPaginationAsync(PaginationDto pagination, string? sortBy = null, string? sortOrder = null, DateTime? startDate = null, DateTime? endDate = null, string? pnr = null, string? agency = null, string? cari = null, decimal? minAmount = null, decimal? maxAmount = null, string? status = null);
        Task<object> GetSalesStatisticsAsync(string period = "monthly", DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetRevenueChartDataAsync(string groupBy = "product", DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> ExportSalesAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string? pnr = null, string? agency = null, string? cari = null);
        Task<Sale> CreateAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Sale?> GetByIdAsync(int id);
        Task<object> GetSummaryAsync();
    }
} 
