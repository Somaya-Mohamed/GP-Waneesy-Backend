using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/video-activities")]
    [Authorize]

    public class VideoActivitiesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VideoActivitiesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/video-activities
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _serviceManager.VideoActivityService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Video activities retrieved successfully",
                Data = activities
            });
        }

        // GET: api/v1/video-activities/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var activity = await _serviceManager.VideoActivityService.GetByIdAsync(id);
            if (activity == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Video activity not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Video activity retrieved successfully",
                Data = activity
            });
        }

        // POST: api/v1/video-activities
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] CreateVideoActivityDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var created = await _serviceManager.VideoActivityService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.ActivityId },
                new
                {
                    Success = true,
                    Message = "Video activity created successfully",
                    Data = created
                });
        }

        // DELETE: api/v1/video-activities/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.VideoActivityService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Video activity not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Video activity deleted successfully"
            });
        }

        // PUT: api/v1/video-activities/5/update-progress
        [HttpPut("{id:int}/update-progress")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateVideoProgressDTO dto)
        {
            var updated = await _serviceManager.VideoActivityService.UpdateProgressAsync(id, dto.WatchPercent, dto.Status);
            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "Video activity not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Progress updated successfully"
            });
        }

        // GET: api/v1/video-activities/child/{childId}  (optional)
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetByChild(int childId)
        {
            var activities = await _serviceManager.VideoActivityService.GetByChildIdAsync(childId);
            return Ok(new
            {
                Success = true,
                Message = "Video activities retrieved successfully",
                Data = activities
            });
        }

        // GET: api/v1/video-activities/video/{videoId}/progress
        [HttpGet("video/{videoId:int}/progress")]
        [Authorize(Roles = "Child,Admin")]
        public async Task<IActionResult> GetProgressByVideo(int videoId)
        {
            var progress = await _serviceManager.VideoActivityService.GetProgressByVideoIdAsync(videoId);
            return Ok(new
            {
                Success = true,
                Message = "Video progress retrieved successfully",
                Data = progress
            });
        }
    }

    public class UpdateVideoProgressDTO
    {
        public double WatchPercent { get; set; }
        public string Status { get; set; }
    }
}
