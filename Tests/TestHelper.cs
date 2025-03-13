using Microsoft.EntityFrameworkCore;
using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    static class TestHelper
    {
        public static ConfigurationDbContext CreateSqlServerDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("MsSqlServer");

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new ConfigurationDbContext(options);

            if (!dbContext.ConfigurationSettings.Any())
            {
                dbContext.ConfigurationSettings.AddRange(new List<ConfigurationSetting>
                {
                    new() { Id = 1, Name = "SiteName", Type = "string", Value = "soty.io", IsActive = true, ApplicationName = "SERVICE-A" },
                    new() { Id = 2, Name = "IsBasketEnabled", Type = "bool", Value = "true", IsActive = true, ApplicationName = "SERVICE-A" },
                    new() { Id = 3, Name = "MaxItemCount", Type = "int", Value = "50", IsActive = false, ApplicationName = "SERVICE-A" },
                    new() { Id = 4, Name = "DiscountRate", Type = "double", Value = "15.5", IsActive = true, ApplicationName = "SERVICE-B" }
                });

                dbContext.SaveChanges();
            }

            return dbContext;
        }
    }
}