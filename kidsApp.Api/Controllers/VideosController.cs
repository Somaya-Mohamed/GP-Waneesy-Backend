using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _videoService.GetAllAsync();
            return Ok(new { Success = true, Data = videos });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var video = await _videoService.GetByIdAsync(id);
            if (video == null) return NotFound(new { Success = false, Message = "Video not found" });
            return Ok(new { Success = true, Data = video });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVideoDTO dto)
        {
            var created = await _videoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.VideoId }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVideoDTO dto)
        {
            var updated = await _videoService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Video not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _videoService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Video not found" });
            return NoContent();
        }

        // Advanced Endpoints

        [HttpGet("{id}/activities")]
        public async Task<IActionResult> GetVideoActivities(int id)
        {
            var activities = await _videoService.GetVideoActivitiesByIdAsync(id);
            return Ok(new { Success = true, Data = activities });
        }

        [HttpGet("difficulty/{level}")]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            var videos = await _videoService.GetVideosByDifficultyAsync(level);
            return Ok(new { Success = true, Data = videos });
        }

        [HttpGet("top-watched")]
        public async Task<IActionResult> GetTopWatched([FromQuery] int topCount = 5)
        {
            var topVideos = await _videoService.GetTopWatchedVideosAsync(topCount);
            return Ok(new { Success = true, Data = topVideos });
        }
    }
}
