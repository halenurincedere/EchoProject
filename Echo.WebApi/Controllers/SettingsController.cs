using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Echo.Business.Operations.Settings;
using Echo.WebApi.Filters;

namespace Echo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(TimeControllerFilter))]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // PATCH: api/settings/toggle
        [HttpPatch("toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleMaintenance()
        {
            await _settingService.ToggleMaintenanceAsync();
            return NoContent();
        }

        // GET: api/settings/status
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var mode = await _settingService.IsMaintenanceModeAsync();
            return Ok(new { maintenanceMode = mode });
        }
    }
}