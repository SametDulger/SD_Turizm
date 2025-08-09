using System.Text.Json;

namespace SD_Turizm.Web.Services
{
    public class LookupApiService : ILookupApiService
    {
        private readonly IApiClientService _apiClient;

        public LookupApiService(IApiClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<dynamic>?> GetCurrenciesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/currencies");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var currencies = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return currencies?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetRoomTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/room-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var roomTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return roomTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetBoardTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/board-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var boardTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return boardTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetVendorTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/vendor-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var vendorTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return vendorTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetSaleStatusesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/sale-statuses");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var saleStatuses = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return saleStatuses?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetPersonTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/person-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var personTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return personTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetRoomLocationsAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/room-locations");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var roomLocations = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return roomLocations?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetProductTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/product-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var productTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return productTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetRolesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/roles");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var roles = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return roles?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> GetTransactionTypesAsync()
        {
            try
            {
                var response = await _apiClient.GetResponseAsync("lookup/transaction-types");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var transactionTypes = JsonSerializer.Deserialize<List<JsonElement>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return transactionTypes?.Cast<dynamic>().ToList();
                }
                return new List<dynamic>();
            }
            catch
            {
                return new List<dynamic>();
            }
        }
    }
}
