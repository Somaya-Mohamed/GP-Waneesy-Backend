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
        // Child يضيف log = "خلصت التاسك ده" ويشوف نقاطه
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (dto.ChildId != childId)
                return Forbid();

            var createdLog = await _serviceManager.TaskLogService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdLog.LogId },
                new

                {
                    Success = true,
                    Message = "Task completed successfully",
                    Data = new
                    {
                        createdLog.LogId,
                        createdLog.ChildId,
                        createdLog.TaskId,
                        createdLog.DateCompleted,
                        createdLog.PointsEarned
                        
                    }
                });
        }

        // DELETE: api/v1/task-logs/5
        // Admin يحذف أي log — Child يحذف log بتاعه بس وبيرجع النقاط
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Child")]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("Child"))
            {
                var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var log = await _serviceManager.TaskLogService.GetByIdAsync(id);
                if (log == null)
                    return NotFound(new { Success = false, Message = "Task log not found" });

                // Child مينفعش يحذف log child تاني
                if (log.ChildId != childId)
                    return Forbid();
            }

            var deleted = await _serviceManager.TaskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log deleted and points deducted successfully" });
        }

        // GET: api/v1/task-logs/task/{taskId}
        [HttpGet("task/{taskId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByTaskIdAsync(taskId);
            return Ok(new { Success = true, Message = "Task logs by task retrieved successfully", Data = logs });
        }

        // GET: api/v1/task-logs/child/{childId}
        // Child يشوف logs بتاعته بس — Parent يشوف logs أولاده بس
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Child,Parent")]
        public async Task<IActionResult> GetByChildId(int childId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (User.IsInRole("Child"))
            {
                if (userId != childId)
                    return Forbid();
            }
            else if (User.IsInRole("Parent"))
            {
                var isOwner = await _serviceManager.ChildService
                    .IsChildBelongsToParentAsync(childId, userId);

                if (!isOwner)
                    return Forbid();
            }

            var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
            return Ok(new { Success = true, Message = "Task logs retrieved successfully", Data = logs });
        }

        // GET: api/v1/task-logs/my-logs
        // Child يشوف كل logs بتاعته + مجموع نقاطه
        [HttpGet("my-logs")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> GetMyLogs()
        {
            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
            var logsList = logs.ToList();
            var totalPoints = logsList.Sum(l => l.PointsEarned);

            return Ok(new
            {
                Success = true,
                Message = "My task logs retrieved successfully",
                TotalPointsEarned = totalPoints,
                Data = logsList
            });
        }
    }
}