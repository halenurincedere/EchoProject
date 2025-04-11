using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Echo.Business.Operations.Feature;
using Echo.Business.Operations.Feature.Dtos;
using Echo.WebApi.Models;

namespace Echo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeaturesController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        // Get all features
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var features = await _featureService.GetAllAsync();
            return Ok(features);
        }

        // Get a feature by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var feature = await _featureService.GetByIdAsync(id);
            if (feature == null)
                return NotFound("No feature found with the specified ID.");

            return Ok(feature);
        }

        // Get a random feature (used for pairing etc.)
        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var feature = await _featureService.GetRandomFeatureAsync();
            if (feature == null)
                return NotFound("No feature available yet.");

            return Ok(feature);
        }

        // Add a new feature (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFeature([FromBody] AddFeatureRequest request)
        {
            var dto = new AddFeatureDto
            {
                Title = request.Title,
                Description = request.Description,
                Note = request.Note,
                Source = request.Source,
                Tag = request.Tag
            };

            var result = await _featureService.AddFeatureAsync(dto);

            if (!result.IsSucceed)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }

        // Update a feature (Admin only)
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchFeature(Guid id, [FromBody] UpdateFeatureRequest request)
        {
            var dto = new UpdateFeatureDto
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Note = request.Note,
                Source = request.Source,
                Tag = request.Tag
            };

            var result = await _featureService.UpdateFeatureAsync(dto);

            if (!result.IsSucceed)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }

        // Soft delete a feature (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeature(Guid id)
        {
            var result = await _featureService.DeleteFeatureAsync(id);

            if (!result.IsSucceed)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}