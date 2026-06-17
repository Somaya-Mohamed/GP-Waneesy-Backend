using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _serviceManager.TaskService.GetAllAsync();
            return Ok(new { Success = true, Message = "Tasks retrieved successfully", Data = tasks });
        }

        // GET: api/v1/tasks/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _serviceManager.TaskService.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new { Success = true, Message = "Task retrieved successfully", Data = task });
        }

        // POST: api/v1/tasks
        // Admin only — creates a Daily task visible to every child
        //[HttpPost]
        [HttpPost("daily")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTaskDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var created = await _serviceManager.TaskService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.TaskId },
                new { Success = true, Message = "Daily task created successfully", Data = created });
        }

        // PUT: api/v1/tasks/5
        [HttpPut("daily/{id:int}")]
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
        [HttpDelete("daily/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.TaskService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new { Success = true, Message = "Task deleted successfully" });
        }

        // GET: api/v1/tasks/difficulty/{level}
        // Admin → يشوف كل التاسكات بالـ difficulty دي
        // Child → يشوف بس تاسكاته اليوم دي (Daily + Personal بتاعته) مفلترة بالـ difficulty
        [HttpGet("difficulty/{level}")]
        [Authorize(Roles = "Admin,Child")]
        public async Task<IActionResult> GetByDifficulty(string level)
        {
            if (string.IsNullOrWhiteSpace(level))
                return BadRequest(new { Success = false, Message = "Difficulty level is required" });

            if (User.IsInRole("Child"))
            {
                var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var todayTasks = await _serviceManager.TaskService.GetTodayTasksForChildAsync(childId);

                var filtered = todayTasks
                    .Where(t => t.Difficulty.Equals(level, StringComparison.OrdinalIgnoreCase));

                return Ok(new { Success = true, Message = "Tasks retrieved by difficulty", Data = filtered });
            }

            // Admin
            var tasks = await _serviceManager.TaskService.GetTasksByDifficultyAsync(level);
            return Ok(new { Success = true, Message = "Tasks retrieved by difficulty", Data = tasks });
        }

        // GET: api/v1/tasks/category/{category}
        // Admin → يشوف كل التاسكات بالـ category دي
        // Child → يشوف بس تاسكاته اليوم دي (Daily + Personal بتاعته) مفلترة بالـ category
        [HttpGet("category/{category}")]
        [Authorize(Roles = "Admin,Child")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { Success = false, Message = "Category is required" });

            if (User.IsInRole("Child"))
            {
                var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var todayTasks = await _serviceManager.TaskService.GetTodayTasksForChildAsync(childId);

                var filtered = todayTasks
                    .Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

                return Ok(new { Success = true, Message = "Tasks retrieved by category", Data = filtered });
            }

            // Admin
            var allTasks = await _serviceManager.TaskService.GetAllAsync();
            var adminFiltered = allTasks
                .Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            return Ok(new { Success = true, Message = "Tasks retrieved by category", Data = adminFiltered });
        }

        // GET: api/v1/tasks/daily
        // Returns the fixed Admin-created Daily tasks (all children share these)
        [HttpGet("daily")]
        [Authorize(Roles = "Admin,Parent,Child")]
        public async Task<IActionResult> GetDailyTasks()
        {
            var tasks = await _serviceManager.TaskService.GetDailyTasksAsync();
            return Ok(new { Success = true, Message = "Daily tasks retrieved successfully", Data = tasks });
        }


        // POST: api/v1/tasks/personal
        // Child adds a Personal task — visible only to them
        [HttpPost("personal")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> CreatePersonal([FromBody] CreatePersonalTaskDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var created = await _serviceManager.TaskService.CreatePersonalTaskAsync(dto, childId);

            return CreatedAtAction(nameof(GetById), new { id = created.TaskId },
                new { Success = true, Message = "Personal task created successfully", Data = created });
        }

        // GET: api/v1/tasks/personal
        // Child sees only their own Personal tasks
        [HttpGet("personal")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> GetMyPersonalTasks()
        {
            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tasks = await _serviceManager.TaskService.GetPersonalTasksByChildIdAsync(childId);

            return Ok(new { Success = true, Message = "Personal tasks retrieved successfully", Data = tasks });
        }

        // DELETE: api/v1/tasks/personal/{id}
        // Child can delete only their own Personal tasks (cannot touch Daily tasks)
        [HttpDelete("personal/{id:int}")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> DeletePersonal(int id)
        {
            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var task = await _serviceManager.TaskService.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { Success = false, Message = "Task not found" });

            if (task.TaskType != "Personal")
                return Forbid(); // child cannot delete Admin Daily tasks

            if (task.CreatedByChildId != childId)
                return Forbid(); // child cannot delete another child's personal tasks

            var deleted = await _serviceManager.TaskService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task not found" });

            return Ok(new { Success = true, Message = "Personal task deleted successfully" });
        }
 
        // GET: api/v1/tasks/today
        [HttpGet("today")]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> GetTodayTasks()
        {
            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tasks = await _serviceManager.TaskService.GetTodayTasksForChildAsync(childId);
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);

            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var todayCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone).Date;

            var todayPoints = logs
                .Where(l => l.DateCompleted.HasValue
                         && TimeZoneInfo.ConvertTimeFromUtc(l.DateCompleted.Value, cairoZone).Date == todayCairo)
                .Sum(l => l.PointsEarned);

            return Ok(new
            {
                Success = true,
                Message = "Today's tasks retrieved successfully",
                TodayPoints = todayPoints,
                Data = tasks
            });
        }


    }
}