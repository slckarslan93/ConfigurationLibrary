using Microsoft.EntityFrameworkCore;
using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;

namespace Tests
{
    static class TestHelper
    {
        public static ConfigurationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseInMemoryDatabase("TestDb")
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
