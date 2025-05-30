﻿namespace ConfigurationLibrary.UI.Models.Configuration
{
    public class ConfigurationAddModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}