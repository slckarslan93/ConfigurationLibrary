public interface IConfigurationReader
{
    /// <summary>
    /// Asynchronously retrieves the value associated with the specified key and converts it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the value should be converted.</typeparam>
    /// <param name="key">The key of the configuration value to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value associated with the specified key, converted to the specified type.</returns>
    Task<T> GetValueAsync<T>(string key);

    /// <summary>
    /// Asynchronously retrieves the value associated with the specified key as an object.
    /// </summary>
    /// <param name="key">The key of the configuration value to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value associated with the specified key as an object.</returns>
    Task<object> GetValueAsync(string key);
}