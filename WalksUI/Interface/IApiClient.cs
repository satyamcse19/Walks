namespace WalksUI.Interface
{
    public interface IApiClient
    {
        Task<IEnumerable<T>> GetAsync<T>(string url);
        Task<T> GetByIdAsync<T>(string url);
        Task<T> PostAsync<T>(string url, T item);
        Task<T> PutAsync<T>(string url, T item);
        Task DeleteAsync(string url);
    }
}
