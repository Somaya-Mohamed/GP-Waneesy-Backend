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
                Message = "Game scores retrieved successfully",
                Data = scores
            });
        }

        // GET: api/v1/game-scores/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var score = await _serviceManager.GameScoreService.GetByIdAsync(id);
            if (score == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game score not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game score retrieved successfully",
                Data = score
            });
        }

        // POST: api/v1/game-scores
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var createdScore = await _serviceManager.GameScoreService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdScore.ScoreId },
                new
                {
                    Success = true,
                    Message = "Game score created successfully",
                    Data = createdScore
                });
        }

        // PUT: api/v1/game-scores/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GameScoreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var updated = await _serviceManager.GameScoreService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game score not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game score updated successfully"
            });
        }

        // DELETE: api/v1/game-scores/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameScoreService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Game score not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Game score deleted successfully"
            });
        }
    }
}
