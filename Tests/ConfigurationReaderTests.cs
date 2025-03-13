using System;
using System.Collections.Generic;
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
        private readonly ConfigurationReader _configReader;

        public ConfigurationReaderTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("MsSqlServer");

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new ConfigurationDbContext(options);
            _configReader = new ConfigurationReader("SERVICE-A", options, 10000);
        }

        [Fact]
        public void GetValue_ShouldReturnStringValue()
        {
            var result = _configReader.GetValue<string>("SiteName");
            Assert.Equal("soty.io", result);
        }

        [Fact]
        public void GetValue_ShouldReturnBoolValue()
        {
            var result = _configReader.GetValue<bool>("IsBasketEnabled");
            Assert.True(result);
        }

        [Fact]
        public void GetValue_ShouldThrowKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => _configReader.GetValue<string>("NonExistentKey"));
        }

        [Fact]
        public void GetValue_ShouldThrowInvalidCastException()
        {
            Assert.Throws<InvalidCastException>(() => _configReader.GetValue<int>("SiteName"));
        }

        [Fact]
        public void GetValue_ShouldIgnoreInactiveRecords()
        {
            Assert.Throws<KeyNotFoundException>(() => _configReader.GetValue<int>("MaxItemCount"));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _configReader.Dispose();
        }
    }
}

