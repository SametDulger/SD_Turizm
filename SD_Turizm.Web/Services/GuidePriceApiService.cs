using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class GuidePriceApiService : IGuidePriceApiService
    {
        private readonly IApiClientService _apiClient;

        public GuidePriceApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<GuidePriceDto>?> GetAllGuidePricesAsync()
        {
            return await _apiClient.GetAsync<List<GuidePriceDto>>("GuidePrice");
        }

        public async Task<GuidePriceDto?> GetGuidePriceByIdAsync(int id)
        {
            return await _apiClient.GetAsync<GuidePriceDto>($"GuidePrice/{id}");
        }

        public async Task<GuidePriceDto?> CreateGuidePriceAsync(GuidePriceDto guidePrice)
        {
            return await _apiClient.PostAsync<GuidePriceDto>("GuidePrice", guidePrice);
        }

        public async Task<GuidePriceDto?> UpdateGuidePriceAsync(int id, GuidePriceDto guidePrice)
        {
            return await _apiClient.PutAsync<GuidePriceDto>($"GuidePrice/{id}", guidePrice);
        }

        public async Task<bool> DeleteGuidePriceAsync(int id)
        {
            return await _apiClient.DeleteAsync($"GuidePrice/{id}");
        }

        public async Task<List<GuidePriceDto>?> GetGuidePricesByGuideIdAsync(int guideId)
        {
            return await _apiClient.GetAsync<List<GuidePriceDto>>($"GuidePrice/guide/{guideId}");
        }
    }
}
