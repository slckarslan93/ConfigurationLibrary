using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary
{
    public class ConfigurationReader : IDisposable
    {
        private readonly string _applicationName;
        private readonly string _connectionString;
        private readonly int _refreshInterval;
        private Timer _timer;
        private List<ConfigurationSetting> _cache = new();

        public ConfigurationReader(string applicationName, string connectionString, int refreshInterval)
        {
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshInterval = refreshInterval;

            LoadConfiguration();
            _timer = new Timer(RefreshConfiguration, null, _refreshInterval, _refreshInterval);
        }

        private void LoadConfiguration()
        {
            try
            {
                using var db = new ConfigurationDbContext(
                    new DbContextOptionsBuilder<ConfigurationDbContext>()
                    .UseSqlServer(_connectionString).Options);

                _cache = db.ConfigurationSettings
                    .Where(c => c.ApplicationName == _applicationName && c.IsActive)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Konfigürasyon yüklenirken hata oluştu: {ex.Message}");
            }
        }

        private void RefreshConfiguration(object state) => LoadConfiguration();

        public T GetValue<T>(string key)
        {
            var setting = _cache.FirstOrDefault(c => c.Name == key);
            if (setting == null) throw new KeyNotFoundException($"Key '{key}' bulunamadı.");

            return ConvertValue<T>(setting.Value);
        }

        private static T ConvertValue<T>(string value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new InvalidCastException($"Değer '{value}' tipi '{typeof(T).Name}' ile uyumsuz.");
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
