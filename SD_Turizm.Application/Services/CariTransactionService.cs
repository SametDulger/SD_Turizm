using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class CariTransactionService : ICariTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CariTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CariTransaction>> GetAllAsync()
        {
            return await _unitOfWork.Repository<CariTransaction>().GetAllAsync();
        }
        public async Task<CariTransaction?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<CariTransaction>().GetByIdAsync(id);
        }
        public async Task<CariTransaction> CreateAsync(CariTransaction entity)
        {
            await _unitOfWork.Repository<CariTransaction>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(CariTransaction entity)
        {
            await _unitOfWork.Repository<CariTransaction>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<CariTransaction>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<CariTransaction>().ExistsAsync(id);
        }

        // V2 Methods
        public async Task<PagedResult<CariTransaction>> GetCariTransactionsWithPaginationAsync(PaginationDto pagination, string? cariCode = null, string? transactionType = null, DateTime? startDate = null, DateTime? endDate = null, decimal? minAmount = null, decimal? maxAmount = null)
        {
            var transactions = await GetAllAsync();
            var transactionsList = transactions.ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(cariCode))
                transactionsList = transactionsList.Where(t => t.CariCode?.Contains(cariCode, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!string.IsNullOrEmpty(transactionType))
                transactionsList = transactionsList.Where(t => t.TransactionType?.Contains(transactionType, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (startDate.HasValue)
                transactionsList = transactionsList.Where(t => t.TransactionDate >= startDate.Value).ToList();
            if (endDate.HasValue)
                transactionsList = transactionsList.Where(t => t.TransactionDate <= endDate.Value).ToList();
            if (minAmount.HasValue)
                transactionsList = transactionsList.Where(t => t.Amount >= minAmount.Value).ToList();
            if (maxAmount.HasValue)
                transactionsList = transactionsList.Where(t => t.Amount <= maxAmount.Value).ToList();

            var totalCount = transactionsList.Count;
            var items = transactionsList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<CariTransaction>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<PagedResult<CariTransaction>> GetCustomerTransactionsAsync(PaginationDto pagination, string cariCode)
        {
            var transactions = await GetAllAsync();
            var transactionsList = transactions.Where(t => t.CariCode == cariCode).ToList();

            var totalCount = transactionsList.Count;
            var items = transactionsList.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return new PagedResult<CariTransaction>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
            };
        }

        public async Task<decimal> GetCustomerBalanceAsync(string cariCode)
        {
            var transactions = await GetAllAsync();
            var customerTransactions = transactions.Where(t => t.CariCode == cariCode).ToList();

            return customerTransactions.Sum(t => t.Amount);
        }

        public async Task<object> GetTransactionStatisticsAsync()
        {
            var transactions = await GetAllAsync();
            var transactionsList = transactions.ToList();

            return new
            {
                TotalTransactions = transactionsList.Count,
                TotalAmount = transactionsList.Sum(t => t.Amount),
                AverageAmount = transactionsList.Any() ? transactionsList.Average(t => t.Amount) : 0,
                TransactionsByType = transactionsList.GroupBy(t => t.TransactionType).Select(g => new { Type = g.Key, Count = g.Count(), Total = g.Sum(t => t.Amount) }).ToList()
            };
        }

        public async Task<byte[]> ExportTransactionsAsync(string format = "excel", string? cariCode = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // Mock implementation
            return await Task.FromResult(System.Text.Encoding.UTF8.GetBytes("Mock transactions export data"));
        }
    }
} 
