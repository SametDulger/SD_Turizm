namespace SD_Turizm.Web.Services
{
    public interface IApiClientService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object data);
        Task<T?> PutAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
        Task<HttpResponseMessage> GetResponseAsync(string endpoint);
        Task<HttpResponseMessage> PostResponseAsync(string endpoint, object data);
        Task<HttpResponseMessage> PutResponseAsync(string endpoint, object data);
        Task<HttpResponseMessage> DeleteResponseAsync(string endpoint);
    }
}