using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/task-logs")]
    [Authorize]
    public class TaskLogsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TaskLogsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/task-logs
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _serviceManager.TaskLogService.GetAllAsync();
            return Ok(new { Success = true, Message = "Task logs retrieved successfully", Data = logs });
        }

        // GET: api/v1/task-logs/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _serviceManager.TaskLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log retrieved successfully", Data = log });
        }

        // POST: api/v1/task-logs
        // Child يضيف log لنفسه بس
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Child مينفعش يضيف log باسم child تاني
            if (dto.ChildId != childId)
                return Forbid();

            var createdLog = await _serviceManager.TaskLogService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdLog.LogId },
                new { Success = true, Message = "Task log created successfully", Data = createdLog });
        }

        // DELETE: api/v1/task-logs/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.TaskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log deleted successfully" });
        }

        // GET: api/v1/task-logs/task/{taskId}
        [HttpGet("task/{taskId:int}")]
        [Authorize(Roles = "Child,Admin")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByTaskIdAsync(taskId);
            return Ok(new { Success = true, Message = "Task logs by task retrieved successfully", Data = logs });
        }

        // GET: api/v1/task-logs/child/{childId}
        // Parent يشوف logs أولاده بس
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var parentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var isOwner = await _serviceManager.ChildService
                .IsChildBelongsToParentAsync(childId, parentId);

            if (!isOwner)
                return Forbid();

            var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
            return Ok(new { Success = true, Message = "Task logs by child retrieved successfully", Data = logs });
        }
    }
}