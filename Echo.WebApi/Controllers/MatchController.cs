using Echo.Business.Operations.Match;
using Echo.Business.Operations.Match.Dtos;
using Echo.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Echo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ Requires authentication for all endpoints
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Creates a match between two specified users (Admin only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")] // 🔐 Only admins can access this
        [ServiceFilter(typeof(TimeControllerFilter))] // ⏱️ Time-based restriction
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _matchService.CreateMatchAsync(dto);

            if (!result.IsSucceed)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }

        /// <summary>
        /// Returns all conversation matches (Admin only).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        /// <summary>
        /// Creates a random match between two users (Admin only).
        /// </summary>
        [HttpPost("random")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(TimeControllerFilter))]
        public async Task<IActionResult> CreateRandomMatch()
        {
            var result = await _matchService.CreateRandomMatchAsync();

            if (!result.IsSucceed)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}