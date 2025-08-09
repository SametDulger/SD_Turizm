using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Sale>> GetSalesReportAsync(DateTime startDate, DateTime endDate, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s =>
                s.CreatedDate >= startDate && s.CreatedDate <= endDate &&
                (string.IsNullOrEmpty(sellerType) || s.SellerType == sellerType) &&
                (string.IsNullOrEmpty(currency) || s.Currency == currency) &&
                (string.IsNullOrEmpty(pnrNumber) || s.PNRNumber == pnrNumber) &&
                (string.IsNullOrEmpty(fileCode) || s.FileCode == fileCode) &&
                (string.IsNullOrEmpty(agencyCode) || s.AgencyCode == agencyCode) &&
                (string.IsNullOrEmpty(cariCode) || s.CariCode == cariCode));

            return sales.OrderByDescending(s => s.CreatedDate);
        }

        public async Task<object> GetSalesSummaryAsync(DateTime startDate, DateTime endDate, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null)
        {
            var sales = await GetSalesReportAsync(startDate, endDate, sellerType, currency, pnrNumber, fileCode, agencyCode, cariCode);

            var summary = new
            {
                TotalSales = sales.Count(),
                TotalSaleAmount = sales.Sum(s => s.SalePrice),
                TotalPurchaseAmount = sales.Sum(s => s.PurchasePrice),
                TotalProfit = sales.Sum(s => s.SalePrice - s.PurchasePrice),
                SalesByType = sales.GroupBy(s => s.SellerType)
                    .Select(g => new
                    {
                        SellerType = g.Key,
                        Count = g.Count(),
                        SaleAmount = g.Sum(s => s.SalePrice),
                        PurchaseAmount = g.Sum(s => s.PurchasePrice),
                        Profit = g.Sum(s => s.SalePrice - s.PurchasePrice)
                    }).ToList()
            };

            return summary;
        }

        public async Task<IEnumerable<Sale>> GetFinancialReportAsync(DateTime startDate, DateTime endDate, string currency = "TRY")
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s =>
                s.CreatedDate >= startDate && s.CreatedDate <= endDate);

            if (currency != "TRY")
            {
                // Döviz kuru dönüşümü için ExchangeRate tablosundan kurları al
                var exchangeRates = await _unitOfWork.Repository<ExchangeRate>().FindAsync(er => 
                    er.Currency == currency && er.Date >= startDate && er.Date <= endDate);

                foreach (var sale in sales)
                {
                    var rate = exchangeRates.FirstOrDefault(er => er.Date.Date == sale.CreatedDate.Date);
                    if (rate != null)
                    {
                        sale.SalePrice = sale.SalePrice * rate.Rate;
                        sale.PurchasePrice = sale.PurchasePrice * rate.Rate;
                    }
                }
            }

            return sales.OrderByDescending(s => s.CreatedDate);
        }

        public async Task<object> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate, string currency = "TRY")
        {
            var sales = await GetFinancialReportAsync(startDate, endDate, currency);

            var summary = new
            {
                TotalRevenue = sales.Sum(s => s.SalePrice),
                TotalCost = sales.Sum(s => s.PurchasePrice),
                TotalProfit = sales.Sum(s => s.SalePrice - s.PurchasePrice),
                ProfitMargin = sales.Any() ? (sales.Sum(s => s.SalePrice - s.PurchasePrice) / sales.Sum(s => s.SalePrice)) * 100 : 0,
                DailyProfits = sales.GroupBy(s => s.CreatedDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Revenue = g.Sum(s => s.SalePrice),
                        Cost = g.Sum(s => s.PurchasePrice),
                        Profit = g.Sum(s => s.SalePrice - s.PurchasePrice)
                    }).OrderBy(x => x.Date).ToList()
            };

            return summary;
        }

        public async Task<IEnumerable<object>> GetCustomerReportAsync(DateTime startDate, DateTime endDate, string? cariCode = null)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s =>
                s.CreatedDate >= startDate && s.CreatedDate <= endDate &&
                (string.IsNullOrEmpty(cariCode) || s.CariCode == cariCode));

            var customerReport = sales.GroupBy(s => new { s.CariCode, s.CustomerName })
                .Select(g => new
                {
                    CariCode = g.Key.CariCode,
                    CustomerName = g.Key.CustomerName,
                    TotalOrders = g.Count(),
                    TotalPurchases = g.Sum(s => s.SalePrice),
                    TotalProfit = g.Sum(s => s.SalePrice - s.PurchasePrice),
                    AverageOrderValue = g.Average(s => s.SalePrice),
                    FirstPurchaseDate = g.Min(s => s.CreatedDate),
                    LastPurchaseDate = g.Max(s => s.CreatedDate),
                    ProductName = g.First().ProductName,
                    SellerType = g.First().SellerType
                })
                .OrderByDescending(x => x.TotalPurchases)
                .ToList();

            return customerReport.Cast<object>();
        }

        public async Task<object> GetCustomerSummaryAsync(DateTime startDate, DateTime endDate, string? cariCode = null)
        {
            var customers = await GetCustomerReportAsync(startDate, endDate, cariCode);
            var customerList = customers.Cast<dynamic>().ToList();

            var summary = new
            {
                TotalCustomers = customerList.Count,
                ActiveCustomers = customerList.Count,
                TotalRevenue = customerList.Sum(c => (decimal)(c.TotalPurchases ?? 0)),
                AverageOrderValue = customerList.Any() ? customerList.Average(c => (decimal)(c.AverageOrderValue ?? 0)) : 0,
                RepeatCustomerRate = customerList.Count > 0 ? customerList.Count(c => (int)(c.TotalOrders ?? 0) > 1) / (double)customerList.Count * 100 : 0,
                CustomerRankings = customerList.OrderByDescending(c => (decimal)(c.TotalPurchases ?? 0)).Take(10).ToList()
            };

            return summary;
        }

        public async Task<IEnumerable<object>> GetProductReportAsync(DateTime startDate, DateTime endDate, string? productType = null)
        {
            var sales = await _unitOfWork.Repository<Sale>().FindAsync(s =>
                s.CreatedDate >= startDate && s.CreatedDate <= endDate &&
                (string.IsNullOrEmpty(productType) || s.SellerType == productType));

            var productReport = sales.GroupBy(s => new { s.SellerType, s.ProductName })
                .Select(g => new
                {
                    ProductType = g.Key.SellerType,
                    ProductName = g.Key.ProductName,
                    TotalSales = g.Count(),
                    TotalRevenue = g.Sum(s => s.SalePrice),
                    TotalCost = g.Sum(s => s.PurchasePrice),
                    TotalProfit = g.Sum(s => s.SalePrice - s.PurchasePrice),
                    AveragePrice = g.Average(s => s.SalePrice)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            return productReport.Cast<object>();
        }

        public async Task<object> GetProductSummaryAsync(DateTime startDate, DateTime endDate, string? productType = null)
        {
            var products = await GetProductReportAsync(startDate, endDate, productType);
            var productList = products.Cast<dynamic>().ToList();

            var summary = new
            {
                TotalProducts = productList.Count,
                TotalSales = productList.Sum(p => (int)p.TotalSales),
                TotalRevenue = productList.Sum(p => (decimal)p.TotalRevenue),
                TotalCost = productList.Sum(p => (decimal)p.TotalCost),
                TotalProfit = productList.Sum(p => (decimal)p.TotalProfit),
                TopProducts = productList.Take(10).ToList()
            };

            return summary;
        }

        // V2 Methods
        public async Task<PagedResult<object>> GetSalesReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null)
        {
            var sales = await GetSalesReportAsync(startDate ?? new DateTime(2025, 1, 1), endDate ?? new DateTime(2025, 12, 31), sellerType, currency, pnrNumber, fileCode, agencyCode, cariCode);
            var salesList = sales.ToList();

            var totalCount = salesList.Count;
            var items = salesList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).Cast<object>().ToList();

            return new PagedResult<object>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<byte[]> ExportSalesReportAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string? sellerType = null, string? currency = null, string? pnrNumber = null, string? fileCode = null, string? agencyCode = null, string? cariCode = null)
        {
            // Mock implementation
            return await Task.FromResult(System.Text.Encoding.UTF8.GetBytes("Mock sales report export data"));
        }

        public async Task<PagedResult<object>> GetFinancialReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string currency = "TRY")
        {
            var sales = await GetFinancialReportAsync(startDate ?? new DateTime(2025, 1, 1), endDate ?? new DateTime(2025, 12, 31), currency);
            var salesList = sales.ToList();

            var totalCount = salesList.Count;
            var items = salesList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).Cast<object>().ToList();

            return new PagedResult<object>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<byte[]> ExportFinancialReportAsync(string format = "excel", DateTime? startDate = null, DateTime? endDate = null, string currency = "TRY")
        {
            // Mock implementation
            return await Task.FromResult(System.Text.Encoding.UTF8.GetBytes("Mock financial report export data"));
        }

        public async Task<PagedResult<object>> GetCustomerReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? cariCode = null)
        {
            var customers = await GetCustomerReportAsync(startDate ?? new DateTime(2025, 1, 1), endDate ?? new DateTime(2025, 12, 31), cariCode);
            var customerList = customers.ToList();

            var totalCount = customerList.Count;
            var items = customerList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<object>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<object>> GetProductReportWithPaginationAsync(PaginationDto pagination, DateTime? startDate = null, DateTime? endDate = null, string? productType = null)
        {
            var products = await GetProductReportAsync(startDate ?? new DateTime(2025, 1, 1), endDate ?? new DateTime(2025, 12, 31), productType);
            var productList = products.ToList();

            var totalCount = productList.Count;
            var items = productList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<object>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<object> GetLiveSalesDataAsync()
        {
            // Mock implementation
            return await Task.FromResult(new
            {
                LiveSales = new[] { 1200, 1500, 1800, 2100, 2400, 2700 },
                TimeLabels = new[] { "09:00", "10:00", "11:00", "12:00", "13:00", "14:00" }
            });
        }

        public async Task<object> GetDashboardWidgetsAsync()
        {
            // Mock implementation
            return await Task.FromResult(new
            {
                SalesWidget = new { Value = 125000, Change = 12.5 },
                RevenueWidget = new { Value = 45000, Change = 8.2 },
                CustomersWidget = new { Value = 1250, Change = 15.3 },
                ProductsWidget = new { Value = 89, Change = -2.1 }
            });
        }

        public async Task<object> GetSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var sales = await GetSalesReportAsync(startDate ?? new DateTime(2025, 1, 1), endDate ?? new DateTime(2025, 12, 31));
            var salesList = sales.ToList();

            return new
            {
                TotalSales = salesList.Count,
                TotalRevenue = salesList.Sum(s => s.SalePrice),
                TotalProfit = salesList.Sum(s => s.SalePrice - s.PurchasePrice),
                AverageAmount = salesList.Any() ? salesList.Average(s => s.SalePrice) : 0
            };
        }
    }
} 
