using ConfigurationLibrary.UI.Models.Configuration;

namespace ConfigurationLibrary.UI.Services.Configuration
{
    public interface IConfigurationService
    {
        Task<ServiceResponse<ConfigurationPaginationModel>> GetPaginationConfigurationAsync(ConfigurationFilterModel filter);

        Task<ServiceResponse> AddConfigurationAsync(ConfigurationAddModel model);

        Task<ServiceResponse> DeleteConfigurationAsync(int id);

        Task<ServiceResponse> ToggleActiveStatusAsync(int id);

        Task<ServiceResponse> UpdateConfigurationAsync(ConfigurationModel model);
    }
}