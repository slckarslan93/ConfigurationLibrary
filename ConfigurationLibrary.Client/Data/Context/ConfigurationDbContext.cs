using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.UI.Data.Context
{
   public  class ConfigurationDbContext:DbContext
    {
        public DbSet<Entities.Configuration.ConfigurationSetting> ConfigurationSettings { get; set; }

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {
            
        }   
    }
}
