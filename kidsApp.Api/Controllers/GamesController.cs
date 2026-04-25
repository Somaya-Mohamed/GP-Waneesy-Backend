using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    [AllowAnonymous]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await _serviceManager.GameService.GetAllAsync();
            return Ok(new { Success = true, Message = "Games retrieved successfully", Data = games });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _serviceManager.GameService.GetByIdAsync(id);
            if (game == null)
                return NotFound(new { Success = false, Message = "Game not found" });

            return Ok(new { Success = true, Message = "Game retrieved successfully", Data = game });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GameCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            if (dto.PointsRewarded < 0)
                return BadRequest(new { Success = false, Message = "PointsRewarded cannot be negative" });

            var createdGame = await _serviceManager.GameService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdGame.GameId },
                new { Success = true, Message = "Game created successfully", Data = createdGame });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GameUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.GameService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Game not found" });

            return Ok(new { Success = true, Message = "Game updated successfully" });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.GameService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Game not found" });

            return Ok(new { Success = true, Message = "Game deleted successfully" });
        }

        // GET: api/v1/games/category/{category}
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { Success = false, Message = "Category is required" });

            var games = await _serviceManager.GameService.GetGamesByCategoryAsync(category);

            var paged = games.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new
            {
                Success = true,
                Message = "Games retrieved successfully by category",
                Page = page,
                PageSize = pageSize,
                Total = games.Count(),
                Data = paged
            });
        }

        // GET: api/v1/games/difficulty/{level}
        [HttpGet("difficulty/{level}")]
        public async Task<IActionResult> GetByDifficulty(string level, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(level))
                return BadRequest(new { Success = false, Message = "Difficulty level is required" });

            var games = await _serviceManager.GameService.GetGamesByDifficultyAsync(level);

            var paged = games.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new
            {
                Success = true,
                Message = "Games retrieved successfully by difficulty",
                Page = page,
                PageSize = pageSize,
                Total = games.Count(),
                Data = paged
            });
        }
    }
}