using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // optional, enable if using JWT auth
    public class TaskLogsController : ControllerBase
    {
        private readonly ITaskLogService _taskLogService;

        public TaskLogsController(ITaskLogService taskLogService)
        {
            _taskLogService = taskLogService;
        }

        // GET: api/TaskLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskLogDTO>>> GetAll()
        {
            var logs = await _taskLogService.GetAllAsync();
            return Ok(logs);
        }

        // GET: api/TaskLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskLogDTO>> GetById(int id)
        {
            var log = await _taskLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound();
            return Ok(log);
        }

        // POST: api/TaskLogs
        [HttpPost]
        public async Task<ActionResult<TaskLogDTO>> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdLog = await _taskLogService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdLog.LogId }, createdLog);
        }

        // DELETE: api/TaskLogs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
