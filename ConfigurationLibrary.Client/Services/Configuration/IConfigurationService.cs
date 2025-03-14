using ConfigurationLibrary.UI.Models.Configuration;

namespace ConfigurationLibrary.UI.Services.Configuration
{
    public interface IConfigurationService
    {
        Task<ServiceResponse<ConfigurationPaginationModel>> GetPaginationConfigurationAsync(ConfigurationFilterModel filter);
    }
}
