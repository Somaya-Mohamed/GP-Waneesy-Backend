using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/game-scores")]
    [Authorize]
    public class GameScoresController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GameScoresController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var scores = await _serviceManager.GameScoreService.GetAllAsync();
            return Ok(new { Success = true, Message = "Game scores retrieved successfully", Data = scores });
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var score = await _serviceManager.GameScoreService.GetByIdAsync(id);
            if (score == null)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new { Success = true, Message = "Game score retrieved successfully", Data = score });
        }

        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var result = await _serviceManager.GameScoreService.CreateAsync(dto);
            return Ok(new { Success = true, Message = "Game score created successfully", Data = result });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GameScoreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.GameScoreService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new { Success = true, Message = "Game score updated successfully" });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameScoreService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new { Success = true, Message = "Game score deleted successfully" });
        }

        // Advanced Endpoints
        [HttpGet("game/{gameId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByGameId(int gameId)
        {
            var scores = await _serviceManager.GameScoreService.GetScoresByGameIdAsync(gameId);
            return Ok(new { Success = true, Message = "Scores by game retrieved successfully", Data = scores });
        }

        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var scores = await _serviceManager.GameScoreService.GetScoresByChildIdAsync(childId);
            return Ok(new { Success = true, Message = "Scores by child retrieved successfully", Data = scores });
        }

        [HttpGet("top/{count:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopScores(int count)
        {
            if (count <= 0) count = 10;
            if (count > 50) count = 50;

            var scores = await _serviceManager.GameScoreService.GetTopScoresAsync(count);
            return Ok(new { Success = true, Message = "Top scores retrieved successfully", Data = scores });
        }
    }
}