using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary
{
    class UsageMethod
    {
        public void Execute()
        {
            var configReader = new ConfigurationReader(
                "SERVICE-A",
                "Server=localhost;Database=ConfigDb;User Id=sa;Password=YourPassword;",
                30000);

                string siteName = configReader.GetValue<string>("SiteName");
                bool isBasketEnabled = configReader.GetValue<bool>("IsBasketEnabled");
                int maxItemCount = configReader.GetValue<int>("MaxItemCount");

                Console.WriteLine($"SiteName: {siteName}");
                Console.WriteLine($"IsBasketEnabled: {isBasketEnabled}");
                Console.WriteLine($"MaxItemCount: {maxItemCount}");
        }
    }
}
