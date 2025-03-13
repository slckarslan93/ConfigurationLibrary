using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLibrary.Data;
using ConfigurationLibrary;

namespace Tests
{
    public class ConfigurationReaderTests :IDisposable
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly ConfigurationReader _configReader;

        public ConfigurationReaderTests()
        {
            _dbContext = TestHelper.CreateInMemoryDbContext();
            _configReader = new ConfigurationReader("SERVICE-A", "", 10000);
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
