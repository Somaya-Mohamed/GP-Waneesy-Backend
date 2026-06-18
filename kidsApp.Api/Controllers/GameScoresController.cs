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

        // POST: api/v1/game-scores  (Child only adds his own score)
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] GameScoreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(childIdClaim, out int currentChildId) || dto.ChildId != currentChildId)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByGameId(int gameId)
        {
            var scores = await _serviceManager.GameScoreService.GetScoresByGameIdAsync(gameId);
            return Ok(new
            {
                Success = true,
                Message = "Scores by game retrieved successfully",
                Data = scores
            });
        }

        // ====================== GET BY CHILD ID (الأهم) ======================
        // GET: api/v1/game-scores/child/{childId}
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Parent,Admin,Child")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
                return Unauthorized(new { Success = false, Message = "Invalid user" });

            if (role == "Child" && currentUserId != childId)
                return Forbid();

            if (role == "Parent")
            {
                var isOwner = await _serviceManager.ChildService
                    .IsChildBelongsToParentAsync(childId, currentUserId);

                if (!isOwner)
                    return Forbid();
            }


            var scores = await _serviceManager.GameScoreService.GetScoresByChildIdAsync(childId);

            return Ok(new
            {
                Success = true,
                Message = "Scores by child retrieved successfully",
                Data = scores
            });
        }

        [HttpGet("my-scores")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> GetMyScores()
        {
            var childIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(childIdClaim, out int childId))
                return Unauthorized(new { Success = false, Message = "Invalid child" });

            var scores = await _serviceManager.GameScoreService.GetMyScoresAsync(childId);

            return Ok(new
            {
                Success = true,
                Message = "My game scores retrieved successfully",
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