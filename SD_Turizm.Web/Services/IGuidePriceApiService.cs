using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IGuidePriceApiService
    {
        Task<List<GuidePriceDto>?> GetAllGuidePricesAsync();
        Task<GuidePriceDto?> GetGuidePriceByIdAsync(int id);
        Task<GuidePriceDto?> CreateGuidePriceAsync(GuidePriceDto guidePrice);
        Task<GuidePriceDto?> UpdateGuidePriceAsync(int id, GuidePriceDto guidePrice);
        Task<bool> DeleteGuidePriceAsync(int id);
        Task<List<GuidePriceDto>?> GetGuidePricesByGuideIdAsync(int guideId);
    }
}
