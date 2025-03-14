using ConfigurationLibrary;
using ConfigurationLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class ConcurrencyTests : IDisposable
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IConfigurationReader _configReader;

        public ConcurrencyTests()
        {
            // appsettings.json dosyasını okuma
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Bağlantı dizesini alma
            var connectionString = configuration.GetConnectionString("MsSqlServer");

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new ConfigurationDbContext(options);
            _configReader = new ConfigurationReader("SERVICE-A", connectionString, 1000);
        }

        [Fact]
        public void MultipleThreads_ShouldNotCauseRaceCondition()
        {
            Parallel.For(0, 100, i =>
            {
                var value = _configReader.GetValue<string>("SiteName");
                Assert.Equal("soty.io", value);
            });
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            (_configReader as IDisposable)?.Dispose();
        }
    }
}

