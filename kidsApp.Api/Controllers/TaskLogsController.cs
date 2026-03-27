using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/task-logs")]
    public class TaskLogsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TaskLogsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _serviceManager.TaskLogService.GetAllAsync();
            return Ok(new { Success = true, Message = "Task logs retrieved successfully", Data = logs });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _serviceManager.TaskLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log retrieved successfully", Data = log });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var createdLog = await _serviceManager.TaskLogService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdLog.LogId },
                new { Success = true, Message = "Task log created successfully", Data = createdLog });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.TaskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log deleted successfully" });
        }

        // GET: api/v1/task-logs/task/{taskId}
        [HttpGet("task/{taskId:int}")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByTaskIdAsync(taskId);
            return Ok(new { Success = true, Message = "Task logs by task retrieved successfully", Data = logs });
        }

        // GET: api/v1/task-logs/child/{childId}   ← مهم جدًا
        [HttpGet("child/{childId:int}")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
            return Ok(new { Success = true, Message = "Task logs by child retrieved successfully", Data = logs });
        }
    }
}