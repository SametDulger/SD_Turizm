using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExchangeRateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ExchangeRate>().GetAllAsync();
        }

        public async Task<ExchangeRate?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ExchangeRate>().GetByIdAsync(id);
        }

        public async Task<ExchangeRate> CreateAsync(ExchangeRate exchangeRate)
        {
            await _unitOfWork.Repository<ExchangeRate>().AddAsync(exchangeRate);
            await _unitOfWork.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task<ExchangeRate> UpdateAsync(ExchangeRate exchangeRate)
        {
            await _unitOfWork.Repository<ExchangeRate>().UpdateAsync(exchangeRate);
            await _unitOfWork.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<ExchangeRate>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<ExchangeRate>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<ExchangeRate>> GetExchangeRatesWithPaginationAsync(PaginationDto pagination, string? fromCurrency = null, string? toCurrency = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var rates = await _unitOfWork.Repository<ExchangeRate>().GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(fromCurrency))
                rates = rates.Where(r => r.FromCurrency.Contains(fromCurrency, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(toCurrency))
                rates = rates.Where(r => r.ToCurrency.Contains(toCurrency, StringComparison.OrdinalIgnoreCase));
            
            if (startDate.HasValue)
                rates = rates.Where(r => r.Date >= startDate.Value);
            
            if (endDate.HasValue)
                rates = rates.Where(r => r.Date <= endDate.Value);

            var totalCount = rates.Count();
            var items = rates.OrderByDescending(r => r.Date).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<ExchangeRate>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<ExchangeRate?> GetLatestRateAsync(string fromCurrency, string toCurrency)
        {
            var rates = await _unitOfWork.Repository<ExchangeRate>().GetAllAsync();
            return rates.Where(r => r.FromCurrency.Equals(fromCurrency, StringComparison.OrdinalIgnoreCase) && 
                                   r.ToCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
                       .OrderByDescending(r => r.Date)
                       .FirstOrDefault();
        }

        public async Task<object> GetExchangeRateStatisticsAsync()
        {
            var rates = await _unitOfWork.Repository<ExchangeRate>().GetAllAsync();
            
            return new
            {
                TotalCount = rates.Count(),
                AverageRate = rates.Any() ? rates.Average(r => r.Rate) : 0,
                MinRate = rates.Any() ? rates.Min(r => r.Rate) : 0,
                MaxRate = rates.Any() ? rates.Max(r => r.Rate) : 0,
                CurrencyPairs = rates.GroupBy(r => new { r.FromCurrency, r.ToCurrency }).Select(g => new { FromCurrency = g.Key.FromCurrency, ToCurrency = g.Key.ToCurrency, Count = g.Count(), AverageRate = g.Average(r => r.Rate) }),
                LatestRates = rates.GroupBy(r => new { r.FromCurrency, r.ToCurrency }).Select(g => g.OrderByDescending(r => r.Date).First())
            };
        }
    }
} 
