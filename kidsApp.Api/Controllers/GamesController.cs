using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGameScoreService _gameScoreService;

        public GameController(IGameService gameService, IGameScoreService gameScoreService)
        {
            _gameService = gameService;
            _gameScoreService = gameScoreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(new { Success = true, Data = games });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound(new { Success = false, Message = "Game not found" });
            return Ok(new { Success = true, Data = game });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateDTO dto)
        {
            var created = await _gameService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.GameId }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateDTO dto)
        {
            var updated = await _gameService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Game not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gameService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Game not found" });
            return NoContent();
        }

        // Advanced Endpoints

        [HttpGet("{id}/scores")]
        public async Task<IActionResult> GetScores(int id)
        {
            var scores = await _gameScoreService.GetScoresByGameIdAsync(id);
            return Ok(new { Success = true, Data = scores });
        }

        [HttpGet("top-scores")]
        public async Task<IActionResult> GetTopScores([FromQuery] int topCount = 5)
        {
            var topScores = await _gameScoreService.GetTopScoresAsync(topCount);
            return Ok(new { Success = true, Data = topScores });
        }

        [HttpGet("difficulty/{level}")]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            var games = await _gameService.GetGamesByDifficultyAsync(level);
            return Ok(new { Success = true, Data = games });
        }
    }
}
