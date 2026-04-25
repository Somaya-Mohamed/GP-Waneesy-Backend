using kidsApp.Application.DTOs.StoryDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/stories")]
    [Authorize]
    public class StoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public StoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/stories
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var stories = await _serviceManager.StoryService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Stories retrieved successfully",
                Data = stories
            });
        }

        // GET: api/v1/stories/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var story = await _serviceManager.StoryService.GetByIdAsync(id);
            if (story == null)
                return NotFound(new { Success = false, Message = "Story not found" });

            return Ok(new
            {
                Success = true,
                Message = "Story retrieved successfully",
                Data = story
            });
        }

        // POST: api/v1/stories
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateStoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.StoryService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.StoryId },
                new { Success = true, Message = "Story created successfully", Data = created });
        }

        // PUT: api/v1/stories/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.StoryService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Story not found" });

            return Ok(new { Success = true, Message = "Story updated successfully" });
        }

        // DELETE: api/v1/stories/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.StoryService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Story not found" });

            return Ok(new { Success = true, Message = "Story deleted successfully" });
        }

        // GET: api/v1/stories/category/{category}
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoriesByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { Success = false, Message = "Category is required" });

            var stories = await _serviceManager.StoryService.GetStoriesByCategoryAsync(category);

            return Ok(new
            {
                Success = true,
                Message = "Stories retrieved successfully by category",
                Data = stories
            });
        }
    }
}