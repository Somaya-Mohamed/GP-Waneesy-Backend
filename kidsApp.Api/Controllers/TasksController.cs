using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(new { Success = true, Data = tasks });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound(new { Success = false, Message = "Task not found" });
            return Ok(new { Success = true, Data = task });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDTO dto)
        {
            var created = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TaskId }, new { Success = true, Data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDTO dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { Success = false, Message = "Task not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            if (!deleted) return NotFound(new { Success = false, Message = "Task not found" });
            return NoContent();
        }

        // Advanced Endpoints
        [HttpGet("{id}/logs")]
        public async Task<IActionResult> GetTaskLogs(int id)
        {
            var logs = await _taskService.GetTaskLogsByTaskIdAsync(id);
            return Ok(new { Success = true, Data = logs });
        }

        [HttpGet("difficulty/{level}")]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            var tasks = await _taskService.GetTasksByDifficultyAsync(level);
            return Ok(new { Success = true, Data = tasks });
        }
    }
}
