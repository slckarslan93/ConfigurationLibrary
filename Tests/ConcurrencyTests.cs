using ConfigurationLibrary;
using ConfigurationLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class ConcurrencyTests : IDisposable
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly IConfigurationReader _configReader;

        public ConcurrencyTests()
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
            _configReader = new ConfigurationReader("SERVICE-A", connectionString, 1000);
        }

        [Fact]
        public async Task MultipleThreads_ShouldNotCauseRaceCondition()
        {
            var tasks = new Task[100];
            for (int i = 0; i < 100; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    var value = await _configReader.GetValueAsync<string>("SiteName");
                    Assert.Equal("soty.io", value);
                });
            }
            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            (_configReader as IDisposable)?.Dispose();
        }
    }
}