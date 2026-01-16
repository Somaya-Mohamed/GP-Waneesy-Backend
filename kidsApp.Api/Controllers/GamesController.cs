using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/games
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _serviceManager.GameService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Games retrieved successfully",
                Data = games
            });
        }

        // GET: api/v1/games/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _serviceManager.GameService.GetByIdAsync(id);
            if (game == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game retrieved successfully",
                Data = game
            });
        }

        // POST: api/v1/games
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var created = await _serviceManager.GameService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.GameId },
                new
                {
                    Success = true,
                    Message = "Game created successfully",
                    Data = created
                });
        }

        // PUT: api/v1/games/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var updated = await _serviceManager.GameService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game updated successfully"
            });
        }

        // DELETE: api/v1/games/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game deleted successfully"
            });
        }

        // ================= Advanced Endpoints =================

        // GET: api/v1/games/5/scores
        [HttpGet("{id:int}/scores")]
        public async Task<IActionResult> GetScores(int id)
        {
            var scores = await _serviceManager.GameScoreService.GetScoresByGameIdAsync(id);
            return Ok(new
            {
                Success = true,
                Message = "Game scores retrieved successfully",
                Data = scores
            });
        }

        // GET: api/v1/games/top-scores?topCount=5
        [HttpGet("top-scores")]
        public async Task<IActionResult> GetTopScores([FromQuery] int topCount = 5)
        {
            if (topCount <= 0) topCount = 5;
            if (topCount > 50) topCount = 50;

            var topScores = await _serviceManager.GameScoreService.GetTopScoresAsync(topCount);
            return Ok(new
            {
                Success = true,
                Message = "Top game scores retrieved successfully",
                Data = topScores
            });
        }

        // GET: api/v1/games/difficulty/easy
        [HttpGet("difficulty/{level}")]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            var games = await _serviceManager.GameService.GetGamesByDifficultyAsync(level);
            return Ok(new
            {
                Success = true,
                Message = "Games retrieved successfully",
                Data = games
            });
        }
    }
}


