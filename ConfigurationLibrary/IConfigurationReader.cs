public interface IConfigurationReader
{
    Task<T> GetValueAsync<T>(string key);
    Task<object> GetValueAsync(string key);
}
