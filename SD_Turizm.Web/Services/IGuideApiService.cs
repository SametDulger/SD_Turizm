using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public interface IGuideApiService
    {
        Task<List<GuideDto>> GetAllGuidesAsync();
        Task<GuideDto?> GetGuideByIdAsync(int id);
        Task<GuideDto?> CreateGuideAsync(GuideDto guide);
        Task<GuideDto?> UpdateGuideAsync(int id, GuideDto guide);
        Task<bool> DeleteGuideAsync(int id);
    }
}
