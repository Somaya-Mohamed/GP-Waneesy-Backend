using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var score = await _serviceManager.GameScoreService.GetByIdAsync(id);
            if (score == null)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new
            {
                Success = true,
                Message = "Game score retrieved successfully",
                Data = score
            });
        }

        // POST: api/v1/game-scores
        // Child يقدر يضيف score لنفسه بس
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Child مينفعش يضيف score باسم child تاني
            if (dto.ChildId != childId)
                return Forbid();

            var result = await _serviceManager.GameScoreService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = result.ScoreId },
                new
                {
                    Success = true,
                    Message = "Game score created successfully",
                    Data = result
                });
        }

        // PUT: api/v1/game-scores/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GameScoreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.GameScoreService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new
            {
                Success = true,
                Message = "Game score updated successfully"
            });
        }

        // DELETE: api/v1/game-scores/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameScoreService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Game score not found" });

            return Ok(new
            {
                Success = true,
                Message = "Game score deleted successfully"
            });
        }

        // GET: api/v1/game-scores/game/{gameId}
        [HttpGet("game/{gameId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByGameId(int gameId)
        {
            var scores = await _serviceManager.GameScoreService.GetScoresByGameIdAsync(gameId);
            return Ok(new { Success = true, Message = "Scores by game retrieved successfully", Data = scores });
        }

        // GET: api/v1/game-scores/child/{childId}
        // Parent يشوف scores أولاده بس
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var parentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isOwner = await _serviceManager.ChildService
                .IsChildBelongsToParentAsync(childId, parentId);

            if (!isOwner)
                return Forbid();

            var scores = await _serviceManager.GameScoreService.GetScoresByChildIdAsync(childId);

            return Ok(new
            {
                Success = true,
                Message = "Scores by child retrieved successfully",
                Data = scores
            });
        }

        // GET: api/v1/game-scores/top/{count}
        [HttpGet("top/{count:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopScores(int count)
        {
            if (count <= 0) count = 10;
            if (count > 50) count = 50;

            var scores = await _serviceManager.GameScoreService.GetTopScoresAsync(count);
            return Ok(new
            {
                Success = true,
                Message = "Top scores retrieved successfully",
                Data = scores
            });
        }
    }
}