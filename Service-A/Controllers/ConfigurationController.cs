using Microsoft.AspNetCore.Mvc;

namespace Service_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationReader _configReader;

        public ConfigurationController(IConfigurationReader configReader)
        {
            _configReader = configReader;
        }

        [HttpGet("{key}")]
        public IActionResult GetValue(string key)
        {
            try
            {
                var value = _configReader.GetValue<string>(key);
                return Ok(value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Key '{key}' not found.");
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
