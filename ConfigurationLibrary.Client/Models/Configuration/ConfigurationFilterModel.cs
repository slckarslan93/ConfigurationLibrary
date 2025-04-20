using ConfigurationLibrary.UI.Models.Pagination;

namespace ConfigurationLibrary.UI.Models.Configuration
{
    public class ConfigurationFilterModel : BasePaginationModel
    {
        public string? Name { get; set; }
        public string? ApplicationName { get; set; }
    }
}