using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // optional, depends if you want JWT auth
    public class GameScoresController : ControllerBase
    {
        private readonly IGameScoreService _gameScoreService;

        public GameScoresController(IGameScoreService gameScoreService)
        {
            _gameScoreService = gameScoreService;
        }

        // GET: api/GameScores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameScoreDTO>>> GetAll()
        {
            var scores = await _gameScoreService.GetAllAsync();
            return Ok(scores);
        }

        // GET: api/GameScores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameScoreDTO>> GetById(int id)
        {
            var score = await _gameScoreService.GetByIdAsync(id);
            if (score == null)
                return NotFound();
            return Ok(score);
        }

        // POST: api/GameScores
        [HttpPost]
        public async Task<ActionResult<GameScoreDTO>> Create([FromBody] GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdScore = await _gameScoreService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdScore.ScoreId }, createdScore);
        }

        // PUT: api/GameScores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameScoreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _gameScoreService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/GameScores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gameScoreService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
