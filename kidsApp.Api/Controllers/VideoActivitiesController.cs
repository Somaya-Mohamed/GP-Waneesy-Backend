using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoActivitiesController : ControllerBase
    {
        private readonly IVideoActivityService _videoActivityService;

        public VideoActivitiesController(IVideoActivityService videoActivityService)
        {
            _videoActivityService = videoActivityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _videoActivityService.GetAllAsync();
            return Ok(new { Success = true, Data = activities });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _videoActivityService.GetByIdAsync(id);
            if (activity == null) return NotFound(new { Success = false, Message = "Activity not found" });
            return Ok(new { Success = true, Data = activity });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVideoActivityDTO dto)
        {
            var created = await _videoActivityService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ActivityId }, new { Success = true, Data = created });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _videoActivityService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Activity not found" });
            return NoContent();
        }

        // Advanced Endpoints

        [HttpPut("{id}/update-progress")]
        public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateVideoProgressDTO dto)
        {
            var updated = await _videoActivityService.UpdateProgressAsync(id, dto.WatchPercent, dto.Status);
            if (!updated) return NotFound(new { Success = false, Message = "Activity not found" });
            return Ok(new { Success = true, Message = "Progress updated successfully" });
        }

        [HttpGet("child/{childId}")]
        public async Task<IActionResult> GetByChild(int childId)
        {
            var activities = await _videoActivityService.GetByChildIdAsync(childId);
            return Ok(new { Success = true, Data = activities });
        }

        [HttpGet("video/{videoId}/progress")]
        public async Task<IActionResult> GetProgressByVideo(int videoId)
        {
            var progress = await _videoActivityService.GetProgressByVideoIdAsync(videoId);
            return Ok(new { Success = true, Data = progress });
        }
    }

    public class UpdateVideoProgressDTO
    {
        public double WatchPercent { get; set; }
        public string Status { get; set; }
    }
}
