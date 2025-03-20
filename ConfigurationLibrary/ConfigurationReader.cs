using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary
{
    public class ConfigurationReader : IConfigurationReader, IDisposable
    {
        private readonly string _applicationName;
        private readonly DbContextOptions<ConfigurationDbContext> _options;
        private readonly int _refreshInterval;
        private Timer _timer;
        private List<ConfigurationSetting> _cache = new();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3);

        public ConfigurationReader(string applicationName, string connectionString, int refreshInterval)
        {
            _applicationName = applicationName;
            _options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            _refreshInterval = refreshInterval;
            LoadConfigurationAsync().Wait();
            _timer = new Timer(RefreshConfigurationAsync, null, _refreshInterval, _refreshInterval); 
        }

        private async Task LoadConfigurationAsync()         
        {
            await _semaphore.WaitAsync();
            try
            {
                using var dbContext = new ConfigurationDbContext(_options);
                _cache = await dbContext.ConfigurationSettings
                    .Where(s => s.ApplicationName == _applicationName && s.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanına erişim hatası: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async void RefreshConfigurationAsync(object state)
        {
            await _semaphore.WaitAsync();
            try
            {
                using var dbContext = new ConfigurationDbContext(_options);
                var newSettings = await dbContext.ConfigurationSettings
                    .Where(s => s.ApplicationName == _applicationName && s.IsActive)
                    .ToListAsync();

                foreach (var setting in newSettings)
                {
                    var existingSetting = _cache.FirstOrDefault(s => s.Name == setting.Name);
                    if (existingSetting == null || existingSetting.Value != setting.Value)
                    {
                        _cache = newSettings;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanına erişim hatası: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<object> GetValueAsync(string key)
        {
            await _semaphore.WaitAsync();
            try
            {
                var type = await GetTypeAsync(key);
                return type.ToLower() switch
                {
                    "int" => await GetValueAsync<int>(key),
                    "bool" => await GetValueAsync<bool>(key),
                    "double" => await GetValueAsync<double>(key),
                    "datetime" => await GetValueAsync<DateTime>(key),
                    "decimal" => await GetValueAsync<decimal>(key),
                    _ => await GetValueAsync<string>(key),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanına erişim hatası: {ex.Message}");
                var setting = _cache.FirstOrDefault(s => s.Name == key);
                if (setting == null)
                {
                    throw new KeyNotFoundException($"Key '{key}' bulunamadı.");
                }
                return ConvertValue(setting.Value, setting.Type);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private object ConvertValue(string value, string type)
        {
            return type.ToLower() switch
            {
                "int" => Convert.ToInt32(value),
                "bool" => value == "1" || bool.Parse(value),
                "double" => Convert.ToDouble(value),
                "datetime" => DateTime.Parse(value),
                "decimal" => Convert.ToDecimal(value),
                _ => value,
            };
        }

        public async Task<string> GetTypeAsync(string key)
        {
            await _semaphore.WaitAsync();
            try
            {
                using var dbContext = new ConfigurationDbContext(_options);
                var setting = await dbContext.ConfigurationSettings.FirstOrDefaultAsync(s => s.Name == key);
                if (setting == null)
                {
                    throw new KeyNotFoundException($"Key '{key}' bulunamadı.");
                }
                return setting.Type;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanına erişim hatası: {ex.Message}");
                var setting = _cache.FirstOrDefault(s => s.Name == key);
                if (setting == null)
                {
                    throw new KeyNotFoundException($"Key '{key}' bulunamadı.");
                }
                return setting.Type;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            await _semaphore.WaitAsync();
            try
            {
                var setting = _cache.FirstOrDefault(s => s.Name == key);
                if (setting == null)
                {
                    throw new KeyNotFoundException($"Key '{key}' bulunamadı.");
                }

                try
                {
                    return ConvertValue<T>(setting.Value);
                }
                catch (FormatException ex)
                {
                    throw new InvalidCastException($"Anahtar '{key}' için değer '{typeof(T).Name}' türüne dönüştürülemez.", ex);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static T ConvertValue<T>(string value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (value == "1")
                {
                    return (T)(object)true;
                }
                if (value == "0")
                {
                    return (T)(object)false;
                }
                return (T)(object)bool.Parse(value);
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void Dispose()
        {
            _semaphore.Dispose();
            _timer?.Dispose();
        }
    }
}





