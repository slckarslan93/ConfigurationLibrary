using ConfigurationLibrary.UI.Models.Configuration;
using ConfigurationLibrary.UI.Services.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationLibrary.UI.Areas.Api
{
    [Area("AdminServices")]
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
    }
}
