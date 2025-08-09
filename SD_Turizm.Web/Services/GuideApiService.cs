using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class GuideApiService : IGuideApiService
    {
        private readonly IApiClientService _apiClient;

        public GuideApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<GuideDto>> GetAllGuidesAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<GuideDto>>("Guide");
            return response?.Items ?? new List<GuideDto>();
        }

        public async Task<GuideDto?> GetGuideByIdAsync(int id)
        {
            return await _apiClient.GetAsync<GuideDto>($"Guide/{id}");
        }

        public async Task<GuideDto?> CreateGuideAsync(GuideDto guide)
        {
            return await _apiClient.PostAsync<GuideDto>("Guide", guide);
        }

        public async Task<GuideDto?> UpdateGuideAsync(int id, GuideDto guide)
        {
            return await _apiClient.PutAsync<GuideDto>($"Guide/{id}", guide);
        }

        public async Task<bool> DeleteGuideAsync(int id)
        {
            return await _apiClient.DeleteAsync($"Guide/{id}");
        }
    }
}
