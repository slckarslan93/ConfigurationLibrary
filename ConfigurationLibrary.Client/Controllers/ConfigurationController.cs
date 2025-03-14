using ConfigurationLibrary.Data;
using ConfigurationLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.UI.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public ConfigurationController(ConfigurationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }







        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var configs = await _context.ConfigurationSettings.ToListAsync();
            return Ok(configs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var config = await _context.ConfigurationSettings.FindAsync(id);
            if (config == null)
                return NotFound();

            return Ok(config);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConfigurationSetting config)
        {
            _context.ConfigurationSettings.Add(config);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = config.Id }, config);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConfigurationSetting config)
        {
            var existing = await _context.ConfigurationSettings.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Value = config.Value;
            existing.IsActive = config.IsActive;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var config = await _context.ConfigurationSettings.FindAsync(id);
            if (config == null) return NotFound();

            _context.ConfigurationSettings.Remove(config);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
