using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetValue(string key)
        {
            try
            {
                var value = await _configReader.GetValueAsync(key); 
                return Ok(value);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Key '{key}' bulunamadı.");
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

    }
}



