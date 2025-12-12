using kidsApp.Application.DTOs.StoryDTOs;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stories = await _storyService.GetAllAsync();
            return Ok(new { Success = true, Data = stories });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            if (story == null) return NotFound(new { Success = false, Message = "Story not found" });
            return Ok(new { Success = true, Data = story });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStoryDTO dto)
        {
            var created = await _storyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.StoryId }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStoryDTO dto)
        {
            var updated = await _storyService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Story not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _storyService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Story not found" });
            return NoContent();
        }

        // Advanced Endpoints
        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetStoryProgress(int id)
        {
            var progress = await _storyService.GetStoryProgressByIdAsync(id);
            return Ok(new { Success = true, Data = progress });
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetStoriesByCategory(string category)
        {
            var stories = await _storyService.GetStoriesByCategoryAsync(category);
            return Ok(new { Success = true, Data = stories });
        }
    }
}
