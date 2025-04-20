using ConfigurationLibrary.UI.Models.Configuration;
using ConfigurationLibrary.UI.Services.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.UI.Areas.Api
{
    [Area("Api")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpPost("pagination")]
        public async Task<IActionResult> Pagination([FromBody] ConfigurationFilterModel filter)
        {
            if (filter == null)
                return BadRequest("Invalid pagination data.");

            var response = await _configurationService.GetPaginationConfigurationAsync(filter);

            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddConfiguration([FromBody] ConfigurationAddModel model)
        {
            if (model == null)
                return BadRequest("Invalid configuration data.");

            var response = await _configurationService.AddConfigurationAsync(model);

            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteConfiguration(int id)
        {
            var response = await _configurationService.DeleteConfigurationAsync(id);

            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("toggle-active/{id}")]
        public async Task<IActionResult> ToggleActiveStatus(int id)
        {
            var response = await _configurationService.ToggleActiveStatusAsync(id);

            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateConfiguration([FromBody] ConfigurationModel model)
        {
            if (model == null)
                return BadRequest("Invalid configuration data.");

            var response = await _configurationService.UpdateConfigurationAsync(model);

            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}