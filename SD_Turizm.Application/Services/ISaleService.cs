using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;

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
    }
} 
