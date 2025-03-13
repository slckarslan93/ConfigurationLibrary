using ConfigurationLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.Data
{
    class ConfigurationDbContext:DbContext
    {
        public DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
            
        }   
    }
}
