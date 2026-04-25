using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TasksController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/tasks
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _serviceManager.TaskService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = tasks
            });
        }

        // GET: api/v1/tasks/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _serviceManager.TaskService.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = task
            });
        }

        // POST: api/v1/tasks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTaskDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.TaskService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.TaskId },
                new { Success = true, Message = "Task created successfully", Data = created });
        }

        // PUT: api/v1/tasks/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var updated = await _serviceManager.TaskService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new { Success = true, Message = "Task updated successfully" });
        }

        // DELETE: api/v1/tasks/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.TaskService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new { Success = true, Message = "Task deleted successfully" });
        }

        // GET: api/v1/tasks/difficulty/{level}
        [HttpGet("difficulty/{level}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            if (string.IsNullOrWhiteSpace(level))
                return BadRequest(new { Success = false, Message = "Difficulty level is required" });

            var tasks = await _serviceManager.TaskService.GetTasksByDifficultyAsync(level);

            return Ok(new
            {
                Success = true,
                Message = "Tasks retrieved successfully by difficulty",
                Data = tasks
            });
        }

        // GET: api/v1/tasks/category/{category}
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { Success = false, Message = "Category is required" });

            var allTasks = await _serviceManager.TaskService.GetAllAsync();

            var filtered = allTasks
                .Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            return Ok(new
            {
                Success = true,
                Message = "Tasks retrieved successfully by category",
                Data = filtered
            });
        }
    }
}