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
        private readonly object _lock = new();

        public ConfigurationReader(string applicationName, string connectionString, int refreshInterval)
        {
            _applicationName = applicationName;
            _options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            _refreshInterval = refreshInterval;
            LoadConfiguration();
            _timer = new Timer(RefreshConfiguration, null, _refreshInterval, _refreshInterval);
        }

        private void LoadConfiguration()
        {
            lock (_lock)
            {
                using var dbContext = new ConfigurationDbContext(_options);
                _cache = dbContext.ConfigurationSettings
                    .Where(s => s.ApplicationName == _applicationName && s.IsActive)
                    .ToList();
            }
        }

        private void RefreshConfiguration(object state)
        {
            lock (_lock)
            {
                using var dbContext = new ConfigurationDbContext(_options);
                var newSettings = dbContext.ConfigurationSettings
                    .Where(s => s.ApplicationName == _applicationName && s.IsActive)
                    .ToList();

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
        }

        public T GetValue<T>(string key)
        {
            lock (_lock)
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
                    throw new InvalidCastException($"Value for key '{key}' cannot be cast to type {typeof(T).Name}.", ex);
                }
            }
        }

        private static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public List<ConfigurationSetting> GetAllSettings()
        {
            lock (_lock)
            {
                return new List<ConfigurationSetting>(_cache);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _timer?.Dispose();
            }
        }
    }
}
