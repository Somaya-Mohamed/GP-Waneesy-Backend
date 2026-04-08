using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/videos")]
    public class VideosController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VideosController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _serviceManager.VideoService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Videos retrieved successfully",
                Data = videos
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var video = await _serviceManager.VideoService.GetByIdAsync(id);
            if (video == null)
                return NotFound(new { Success = false, Message = "Video not found" });

            return Ok(new
            {
                Success = true,
                Message = "Video retrieved successfully",
                Data = video
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVideoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.VideoService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new { Success = true, Message = "Video created successfully", Data = created });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVideoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.VideoService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Video not found" });

            return Ok(new { Success = true, Message = "Video updated successfully" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.VideoService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Video not found" });

            return Ok(new { Success = true, Message = "Video deleted successfully" });
        }

        // GET: api/v1/videos/{id}/activities
        [HttpGet("{id:int}/activities")]
        public async Task<IActionResult> GetVideoActivities(int id)
        {
            var activities = await _serviceManager.VideoService.GetVideoActivitiesByIdAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Video activities retrieved successfully",
                Data = activities
            });
        }

        // GET: api/v1/videos/category/{category}
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { Success = false, Message = "Category is required" });

            var videos = await _serviceManager.VideoService.GetVideosByCategoryAsync(category);
            return Ok(new
            {
                Success = true,
                Message = "Videos retrieved successfully by category",
                Data = videos
            });
        }

        // GET: api/v1/videos/top-watched
        [HttpGet("top-watched")]
        public async Task<IActionResult> GetTopWatched([FromQuery] int topCount = 5)
        {
            if (topCount <= 0) topCount = 5;
            if (topCount > 20) topCount = 20;

            var topVideos = await _serviceManager.VideoService.GetTopWatchedVideosAsync(topCount);
            return Ok(new
            {
                Success = true,
                Message = "Top watched videos retrieved successfully",
                Data = topVideos
            });
        }
    }
}