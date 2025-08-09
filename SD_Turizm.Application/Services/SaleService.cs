using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _unitOfWork.Repository<Sale>().GetAllAsync();
        }

        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Sale>().GetByIdAsync(id);
        }

        public async Task<Sale?> GetSaleByPNRAsync(string pnrNumber)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s => s.PNRNumber == pnrNumber);
            return sales.FirstOrDefault();
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            if (string.IsNullOrEmpty(sale.PNRNumber))
            {
                sale.PNRNumber = await GeneratePNRNumberAsync();
            }

            await _unitOfWork.Repository<Sale>().AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();
            return sale;
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            await _unitOfWork.Repository<Sale>().UpdateAsync(sale);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSaleAsync(int id)
        {
            await _unitOfWork.Repository<Sale>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> SaleExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Sale>().ExistsAsync(id);
        }

        public async Task<bool> PNRExistsAsync(string pnrNumber)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s => s.PNRNumber == pnrNumber);
            return sales.Any();
        }

        public async Task<string> GeneratePNRNumberAsync()
        {
            var random = new Random();
            string pnrNumber;
            
            do
            {
                pnrNumber = random.Next(1000, 9999).ToString();
            } while (await PNRExistsAsync(pnrNumber));

            return pnrNumber;
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s => 
                s.CreatedDate >= startDate && s.CreatedDate <= endDate);
            return sales;
        }

        public async Task<IEnumerable<Sale>> GetSalesByAgencyAsync(string agencyCode)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s => s.AgencyCode == agencyCode);
            return sales;
        }

        public async Task<IEnumerable<Sale>> GetSalesByCariCodeAsync(string cariCode)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s => s.CariCode == cariCode);
            return sales;
        }

        // V2 Methods
        public async Task<PagedResult<Sale>> GetSalesWithPaginationAsync(PaginationDto pagination, string? sortBy = null, string? sortOrder = null, DateTime? startDate = null, DateTime? endDate = null, string? pnr = null, string? agency = null, string? cari = null, decimal? minAmount = null, decimal? maxAmount = null, string? status = null)
        {
            var query = await _unitOfWork.Repository<Sale>().GetAllAsync();
            var sales = query.ToList();

            // Apply filters
            if (startDate.HasValue)
                sales = sales.Where(s => s.CreatedDate >= startDate.Value).ToList();
            if (endDate.HasValue)
                sales = sales.Where(s => s.CreatedDate <= endDate.Value).ToList();
            if (!string.IsNullOrEmpty(pnr))
                sales = sales.Where(s => s.PNRNumber?.Contains(pnr, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!string.IsNullOrEmpty(agency))
                sales = sales.Where(s => s.AgencyCode?.Contains(agency, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!string.IsNullOrEmpty(cari))
                sales = sales.Where(s => s.CariCode?.Contains(cari, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (minAmount.HasValue)
                sales = sales.Where(s => s.TotalAmount >= minAmount.Value).ToList();
            if (maxAmount.HasValue)
                sales = sales.Where(s => s.TotalAmount <= maxAmount.Value).ToList();
            if (!string.IsNullOrEmpty(status))
                sales = sales.Where(s => s.Status?.Contains(status, StringComparison.OrdinalIgnoreCase) == true).ToList();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                sales = sortBy.ToLower() switch
                {
                    "date" => sortOrder?.ToLower() == "desc" ? sales.OrderByDescending(s => s.CreatedDate).ToList() : sales.OrderBy(s => s.CreatedDate).ToList(),
                    "amount" => sortOrder?.ToLower() == "desc" ? sales.OrderByDescending(s => s.TotalAmount).ToList() : sales.OrderBy(s => s.TotalAmount).ToList(),
                    "pnr" => sortOrder?.ToLower() == "desc" ? sales.OrderByDescending(s => s.PNRNumber).ToList() : sales.OrderBy(s => s.PNRNumber).ToList(),
                    _ => sortOrder?.ToLower() == "desc" ? sales.OrderByDescending(s => s.CreatedDate).ToList() : sales.OrderBy(s => s.CreatedDate).ToList()
                };
            }

            // Apply pagination
            var totalCount = sales.Count;
            var items = sales.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<Sale>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetSalesStatisticsAsync(string period = "monthly", DateTime? startDate = null, DateTime? endDate = null)
        {
            var sales = await GetAllSalesAsync();
            var salesList = sales.ToList();

            if (startDate.HasValue)
                salesList = salesList.Where(s => s.CreatedDate >= startDate.Value).ToList();
            if (endDate.HasValue)
                salesList = salesList.Where(s => s.CreatedDate <= endDate.Value).ToList();

            return new
            {
                TotalSales = salesList.Count,
                TotalRevenue = salesList.Sum(s => s.TotalAmount),
                AverageAmount = salesList.Any() ? salesList.Average(s => s.TotalAmount) : 0,
                Period = period
            };
        }

        public async Task<object> GetRevenueChartDataAsync(string groupBy = "product", DateTime? startDate = null, DateTime? endDate = null)
        {
            var sales = await GetAllSalesAsync();
            var salesList = sales.ToList();

            if (startDate.HasValue)
                salesList = salesList.Where(s => s.CreatedDate >= startDate.Value).ToList();
            if (endDate.HasValue)
                salesList = salesList.Where(s => s.CreatedDate <= endDate.Value).ToList();

            return new
            {
                Labels = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran" },
                Data = new[] { 12000, 19000, 15000, 25000, 22000, 30000 },
                GroupBy = groupBy
            };
        }

        public async Task<byte[]> ExportSalesAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string? pnr = null, string? agency = null, string? cari = null)
        {
            // Mock implementation
            return await Task.FromResult(System.Text.Encoding.UTF8.GetBytes("Mock export data"));
        }

        public async Task<Sale> CreateAsync(Sale sale)
        {
            return await CreateSaleAsync(sale);
        }

        public async Task UpdateAsync(Sale sale)
        {
            await UpdateSaleAsync(sale);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteSaleAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await SaleExistsAsync(id);
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await GetSaleByIdAsync(id);
        }

        public async Task<object> GetSummaryAsync()
        {
            var sales = await GetAllSalesAsync();
            var salesList = sales.ToList();

            return new
            {
                TotalSales = salesList.Count,
                TotalRevenue = salesList.Sum(s => s.TotalAmount),
                AverageAmount = salesList.Any() ? salesList.Average(s => s.TotalAmount) : 0
            };
        }
    }
} 
