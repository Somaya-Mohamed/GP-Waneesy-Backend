using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var logs = await _serviceManager.TaskLogService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Message = "Task logs retrieved successfully",
                Data = logs
            });
        }

        // GET: api/v1/task-logs/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _serviceManager.TaskLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Task log not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Task log retrieved successfully",
                Data = log
            });
        }

        // POST: api/v1/task-logs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid data",
                    Errors = ModelState
                });

            var createdLog = await _serviceManager.TaskLogService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdLog.LogId },
                new
                {
                    Success = true,
                    Message = "Task log created successfully",
                    Data = createdLog
                });
        }

        // DELETE: api/v1/task-logs/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.TaskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new
                {
                    Success = false,
                    Message = "Task log not found"
                });

            return Ok(new
            {
                Success = true,
                Message = "Task log deleted successfully"
            });
        }
    }
}
