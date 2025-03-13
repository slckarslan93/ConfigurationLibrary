using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ConfigurationApi.Controllers;

namespace Tests
{
    public class ConfigurationApiControllerTests : IDisposable
    {
        private readonly ConfigurationDbContext _dbContext;
        private readonly ConfigurationController _controller;

        public ConfigurationApiControllerTests()
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
            _controller = new ConfigurationController(_dbContext);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllConfigurations()
        {
            var result = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsType<List<ConfigurationSetting>>(okResult.Value);

            Assert.Equal(4, items.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnConfiguration_WhenIdExists()
        {
            var result = await _controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var config = Assert.IsType<ConfigurationSetting>(okResult.Value);

            Assert.Equal("SiteName", config.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var result = await _controller.GetById(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldAddNewConfiguration()
        {
            var newConfig = new ConfigurationSetting
            {
                Name = "NewSetting",
                Type = "string",
                Value = "TestValue",
                IsActive = true,
                ApplicationName = "SERVICE-C"
            };

            var result = await _controller.Create(newConfig);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedConfig = Assert.IsType<ConfigurationSetting>(createdResult.Value);

            Assert.Equal("NewSetting", returnedConfig.Name);
            Assert.Equal(5, await _dbContext.ConfigurationSettings.CountAsync());
        }

        [Fact]
        public async Task Update_ShouldModifyExistingConfiguration()
        {
            var updatedConfig = new ConfigurationSetting
            {
                Id = 1,
                Name = "SiteName",
                Type = "string",
                Value = "updated.io",
                IsActive = true,
                ApplicationName = "SERVICE-A"
            };

            var result = await _controller.Update(1, updatedConfig);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var config = Assert.IsType<ConfigurationSetting>(okResult.Value);

            Assert.Equal("updated.io", config.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var newConfig = new ConfigurationSetting
            {
                Id = 999,
                Name = "NonExisting",
                Type = "string",
                Value = "NoValue",
                IsActive = true,
                ApplicationName = "SERVICE-X"
            };

            var result = await _controller.Update(999, newConfig);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveConfiguration()
        {
            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);

            var config = await _dbContext.ConfigurationSettings.FindAsync(1);
            Assert.Null(config);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var result = await _controller.Delete(999);
            Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}


