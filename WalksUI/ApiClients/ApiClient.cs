using System.Net.Http;
using System.Text;
using System.Text.Json;
using WalksUI.Interface;
using WalksUI.Models.Domain;
using static System.Net.WebRequestMethods;

namespace WalksUI.ApiClients
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClient;

        public ApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(string url)
        {
           
            try
            {
                var Client = _httpClient.CreateClient();
                var httpResponseMessage = await Client.GetAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();
               return await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<T>>();
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Error deserializing JSON response", jsonEx);
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Error calling API", httpEx);
            }
        }

        public async Task<T> GetByIdAsync<T>(string url)
        {
            try
            {
                var client = _httpClient.CreateClient();
                return await client.GetFromJsonAsync<T>(url);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error retrieving data: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred: " + ex.Message, ex);
            }
        }


        public async Task<T> PostAsync<T>(string url, T item)
        {
            var client = _httpClient.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task<T> PutAsync<T>(string url, T item)
        {
            var client = _httpClient.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task DeleteAsync(string url)
        {
            var Client = _httpClient.CreateClient();
            var response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
