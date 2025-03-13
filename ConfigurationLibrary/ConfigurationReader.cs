using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.Data;

namespace ConfigurationLibrary
{
    public class ConfigurationReader : IDisposable
    {
        private readonly string _applicationName;
        private readonly ConfigurationDbContext _dbContext;
        private readonly int _refreshInterval;
        private Timer _timer;
        private List<ConfigurationSetting> _cache = new();

        public ConfigurationReader(string applicationName, DbContextOptions<ConfigurationDbContext> options, int refreshInterval)
        {
            _applicationName = applicationName;
            _dbContext = new ConfigurationDbContext(options);
            _refreshInterval = refreshInterval;
            LoadConfiguration();
            _timer = new Timer(RefreshConfiguration, null, _refreshInterval, _refreshInterval);
        }

        private void LoadConfiguration()
        {
            _cache = _dbContext.ConfigurationSettings
                .Where(s => s.ApplicationName == _applicationName && s.IsActive)
                .ToList();
        }

        private void RefreshConfiguration(object state)
        {
            LoadConfiguration();
        }

        public T GetValue<T>(string key)
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

        private static T ConvertValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _dbContext?.Dispose();
        }
    }
}

