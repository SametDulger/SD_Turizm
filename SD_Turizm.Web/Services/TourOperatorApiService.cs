using SD_Turizm.Web.Models.DTOs;

namespace SD_Turizm.Web.Services
{
    public class TourOperatorApiService : ITourOperatorApiService
    {
        private readonly IApiClientService _apiClient;

        public TourOperatorApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<TourOperatorDto>> GetAllTourOperatorsAsync()
        {
            var response = await _apiClient.GetAsync<PaginatedResponse<TourOperatorDto>>("TourOperators");
            return response?.Items ?? new List<TourOperatorDto>();
        }

        public async Task<TourOperatorDto?> GetTourOperatorByIdAsync(int id)
        {
            return await _apiClient.GetAsync<TourOperatorDto>($"TourOperators/{id}");
        }

        public async Task<TourOperatorDto?> CreateTourOperatorAsync(TourOperatorDto tourOperator)
        {
            return await _apiClient.PostAsync<TourOperatorDto>("TourOperators", tourOperator);
        }

        public async Task<TourOperatorDto?> UpdateTourOperatorAsync(int id, TourOperatorDto tourOperator)
        {
            return await _apiClient.PutAsync<TourOperatorDto>($"TourOperators/{id}", tourOperator);
        }

        public async Task<bool> DeleteTourOperatorAsync(int id)
        {
            return await _apiClient.DeleteAsync($"TourOperators/{id}");
        }
    }
}
