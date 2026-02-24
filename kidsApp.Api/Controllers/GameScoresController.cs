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

        // GET: api/v1/game-scores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var scores = await _serviceManager.GameScoreService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Data = scores
            });
        }

        // GET: api/v1/game-scores/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var score = await _serviceManager.GameScoreService.GetByIdAsync(id);
            if (score == null)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new { Success = true, Data = score });
        }

        // POST: api/v1/game-scores
        [HttpPost]
        public async Task<IActionResult> Create(GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.GameScoreService.CreateAsync(dto);
            return Ok(new { Success = true, Data = result });
        }

        // PUT: api/v1/game-scores/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, GameScoreUpdateDTO dto)
        {
            var updated = await _serviceManager.GameScoreService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false });

            return Ok(new { Success = true });
        }

        // DELETE: api/v1/game-scores/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameScoreService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false });

            return Ok(new { Success = true });
        }

        // ============================
        // ADVANCED (Child-style simple)
        // ============================

        // GET: api/v1/game-scores/game/{gameId}
        [HttpGet("game/{gameId:int}")]
        public async Task<IActionResult> GetByGameId(int gameId)
        {
            var scores = await _serviceManager.GameScoreService
                .GetScoresByGameIdAsync(gameId);

            return Ok(new
            {
                Success = true,
                Data = scores
            });
        }

        // GET: api/v1/game-scores/top/{count}
        [HttpGet("top/{count:int}")]
        public async Task<IActionResult> GetTopScores(int count)
        {
            var scores = await _serviceManager.GameScoreService
                .GetTopScoresAsync(count);

            return Ok(new
            {
                Success = true,
                Data = scores
            });
        }
    }
}
