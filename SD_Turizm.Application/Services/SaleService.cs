using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

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
    }
} 
