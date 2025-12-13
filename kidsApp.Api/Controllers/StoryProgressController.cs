using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // optional, depends on JWT auth
    public class StoryProgressController : ControllerBase
    {
        private readonly IStoryProgressService _storyProgressService;

        public StoryProgressController(IStoryProgressService storyProgressService)
        {
            _storyProgressService = storyProgressService;
        }

        // GET: api/StoryProgress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryProgressDTO>>> GetAll()
        {
            var progresses = await _storyProgressService.GetAllAsync();
            return Ok(progresses);
        }

        // GET: api/StoryProgress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoryProgressDTO>> GetById(int id)
        {
            var progress = await _storyProgressService.GetByIdAsync(id);
            if (progress == null)
                return NotFound();
            return Ok(progress);
        }

        // POST: api/StoryProgress
        [HttpPost]
        public async Task<ActionResult<StoryProgressDTO>> Create([FromBody] CreateStoryProgressDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProgress = await _storyProgressService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProgress.ProgressId }, createdProgress);
        }

        // DELETE: api/StoryProgress/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _storyProgressService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
