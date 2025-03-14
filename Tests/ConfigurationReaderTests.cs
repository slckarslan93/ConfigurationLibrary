using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ConfigurationLibrary;
using ConfigurationLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tests
{
    public class ConfigurationReaderTests : IDisposable
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IConfigurationReader _configReader;

        public ConfigurationReaderTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ConfigurationConnection");

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new ConfigurationDbContext(options);
            _configReader = new ConfigurationReader("SERVICE-A", connectionString, 10000);
        }

        [Fact]
        public async Task GetValueAsync_ShouldReturnStringValue()
        {
            var result = await _configReader.GetValueAsync<string>("SiteName");
            Assert.Equal("soty.io", result);
        }

        [Fact]
        public async Task GetValueAsync_ShouldReturnBoolValue()
        {
            var result = await _configReader.GetValueAsync<bool>("IsBasketEnabled");
            Assert.True(result);
        }

        [Fact]
        public async Task GetValueAsync_ShouldThrowKeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _configReader.GetValueAsync<string>("NonExistentKey"));
        }

        [Fact]
        public async Task GetValueAsync_ShouldThrowInvalidCastException()
        {
            await Assert.ThrowsAsync<InvalidCastException>(() => _configReader.GetValueAsync<int>("SiteName"));
        }

        [Fact]
        public async Task GetValueAsync_ShouldIgnoreInactiveRecords()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _configReader.GetValueAsync<int>("MaxItemCount"));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            (_configReader as IDisposable)?.Dispose();
        }
    }
}


