using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/story-progress")]
    public class StoryProgressController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public StoryProgressController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/story-progress
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var progresses = await _serviceManager.StoryProgressService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Story progress retrieved successfully",
                Data = progresses
            });
        }

        // GET: api/v1/story-progress/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var progress = await _serviceManager.StoryProgressService.GetByIdAsync(id);
            if (progress == null)
                return NotFound(new { Success = false, Message = "Story progress not found" });

            return Ok(new
            {
                Success = true,
                Message = "Story progress retrieved successfully",
                Data = progress
            });
        }

        // GET: api/v1/story-progress/story/{storyId}  
        [HttpGet("story/{storyId:int}")]
        public async Task<IActionResult> GetByStoryId(int storyId)
        {
            var progress = await _serviceManager.StoryProgressService.GetStoryProgressByIdAsync(storyId);
            return Ok(new
            {
                Success = true,
                Message = "Story progress retrieved successfully",
                Data = progress
            });
        }

        // NEW: GET: api/v1/story-progress/child/{childId}  
        [HttpGet("child/{childId:int}")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var progress = await _serviceManager.StoryProgressService.GetProgressByChildIdAsync(childId);
            return Ok(new
            {
                Success = true,
                Message = "Child story progress retrieved successfully",
                Data = progress
            });
        }

        // POST: api/v1/story-progress
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStoryProgressDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var createdProgress = await _serviceManager.StoryProgressService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdProgress.ProgressId },
                new
                {
                    Success = true,
                    Message = "Story progress created successfully",
                    Data = createdProgress
                });
        }

        // DELETE: api/v1/story-progress/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.StoryProgressService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Story progress not found" });

            return Ok(new { Success = true, Message = "Story progress deleted successfully" });
        }
    }
}