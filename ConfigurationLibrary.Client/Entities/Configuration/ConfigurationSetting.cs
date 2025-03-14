using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary.UI.Entities.Configuration
{
    public class ConfigurationSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
