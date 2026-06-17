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

        // GET: api/v1/task-logs/task/{taskId}
        [HttpGet("task/{taskId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            var logs = await _serviceManager.TaskLogService.GetTaskLogsByTaskIdAsync(taskId);
            return Ok(new { Success = true, Message = "Task logs by task retrieved successfully", Data = logs });
        }

        // POST: api/v1/task-logs
        // Child submits a task completion (Daily or Personal).
        // Points are awarded immediately and stored on the Child entity.
        // At midnight the Completed log is deleted → the task becomes available again next day.
        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> Create([FromBody] CreateTaskLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid data", Errors = ModelState });

            var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (dto.ChildId != childId)
                return Forbid();

            try
            {
                var createdLog = await _serviceManager.TaskLogService.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = createdLog.LogId },
                    new
                    {
                        Success = true,
                        Message = "Task completed successfully! Points have been added.",
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
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
        }


        // DELETE: api/v1/task-logs/5
        // Admin can delete any log.
        // Child can delete only their own log (points will be deducted by the caller if needed).
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

                if (log.ChildId != childId)
                    return Forbid();
            }

            var deleted = await _serviceManager.TaskLogService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false, Message = "Task log not found" });

            return Ok(new { Success = true, Message = "Task log deleted successfully" });
        }


        // GET: api/v1/task-logs/child/{childId}?includeHistory=true&days=30
        [HttpGet("child/{childId:int}")]
        [Authorize(Roles = "Child,Parent,Admin")]
        public async Task<IActionResult> GetByChildId(
            int childId,
            [FromQuery] bool includeHistory = false,
            [FromQuery] int days = 30)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (User.IsInRole("Child") && userId != childId)
                return Forbid();

            if (User.IsInRole("Parent"))
            {
                var isOwner = await _serviceManager.ChildService
                    .IsChildBelongsToParentAsync(childId, userId);
                if (!isOwner) return Forbid();
            }

            // Active logs دايماً
            var activeLogs = (await _serviceManager.TaskLogService
                .GetTaskLogsByChildIdAsync(childId)).ToList();

            //var todayPoints = activeLogs
            //    .Where(l => l.DateCompleted.HasValue
            //             && l.DateCompleted.Value.Date == DateTime.UtcNow.Date)
            //    .Sum(l => l.PointsEarned);

            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var todayCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone).Date;

            var todayPoints = activeLogs
                .Where(l => l.DateCompleted.HasValue
                         && TimeZoneInfo.ConvertTimeFromUtc(l.DateCompleted.Value, cairoZone).Date == todayCairo)
                .Sum(l => l.PointsEarned);

            if (!includeHistory)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Task logs retrieved successfully",
                    TodayPoints = todayPoints,
                    TotalPointsEarned = activeLogs.Sum(l => l.PointsEarned),
                    Data = activeLogs
                });
            }

            // Active + Archived مع بعض
            var historyLogs = (await _serviceManager.TaskLogService
                .GetChildHistoryAsync(childId, days)).ToList();

            var allLogs = activeLogs
                .Concat(historyLogs)
                .OrderByDescending(l => l.DateCompleted)
                .ToList();

            return Ok(new
            {
                Success = true,
                Message = $"Task logs with history (last {days} days)",
                TodayPoints = todayPoints,
                TotalPointsEarned = allLogs.Sum(l => l.PointsEarned),
                TotalTasksCompleted = historyLogs.Count + activeLogs.Count,
                ActiveLogs = activeLogs,
                HistoryLogs = historyLogs
            });
        }



        //// GET: api/v1/task-logs/child/{childId}
        //// Child sees only their own logs.
        //// Parent sees only logs of their own children.
        //[HttpGet("childd/{childId:int}")]
        //[Authorize(Roles = "Child,Parent")]
        //public async Task<IActionResult> GetByChildId(int childId)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        //    if (User.IsInRole("Child") && userId != childId)
        //        return Forbid();

        //    if (User.IsInRole("Parent"))
        //    {
        //        var isOwner = await _serviceManager.ChildService
        //            .IsChildBelongsToParentAsync(childId, userId);

        //        if (!isOwner)
        //            return Forbid();
        //    }

        //    var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
        //    return Ok(new { Success = true, Message = "Task logs retrieved successfully", Data = logs });
        //}

        //////GET: api/v1/task-logs/child/{childId}
        ////[HttpGet("child/{childId:int}")]
        ////[Authorize(Roles = "Child,Parent")]
        ////public async Task<IActionResult> GetByChildId(int childId)
        ////{
        ////    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        ////    if (User.IsInRole("Child") && userId != childId)
        ////        return Forbid();

        ////    if (User.IsInRole("Parent"))
        ////    {
        ////        var isOwner = await _serviceManager.ChildService
        ////            .IsChildBelongsToParentAsync(childId, userId);
        ////        if (!isOwner) return Forbid();
        ////    }

        ////    var logs = await _serviceManager.TaskLogService
        ////        .GetTaskLogsByChildIdAsync(childId);
        ////    var logsList = logs.ToList();

        ////    var todayPoints = logsList
        ////        .Where(l => l.DateCompleted.HasValue
        ////                 && l.DateCompleted.Value.Date == DateTime.UtcNow.Date)
        ////        .Sum(l => l.PointsEarned);

        ////    return Ok(new
        ////    {
        ////        Success = true,
        ////        Message = "Task logs retrieved successfully",
        ////        TodayPoints = todayPoints,
        ////        TotalPointsEarned = logsList.Sum(l => l.PointsEarned),
        ////        Data = logsList
        ////    });
        ////}




        ////// GET: api/v1/task-logs/my-logs
        ////// Child sees all their own logs + total points earned so far today.
        ////[HttpGet("my-logs")]
        ////[Authorize(Roles = "Child")]
        ////public async Task<IActionResult> GetMyLogs()
        ////{
        ////    var childId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        ////    var logs = await _serviceManager.TaskLogService.GetTaskLogsByChildIdAsync(childId);
        ////    var logsList = logs.ToList();
        ////    var totalPoints = logsList.Sum(l => l.PointsEarned);

        ////    var todayPoints = logsList
        ////        .Where(l => l.DateCompleted.HasValue
        ////                 && l.DateCompleted.Value.Date == DateTime.UtcNow.Date)
        ////        .Sum(l => l.PointsEarned);

        ////    return Ok(new
        ////    {
        ////        Success = true,
        ////        Message = "My task logs retrieved successfully",
        ////        TodayPoints = todayPoints,
        ////        TotalPointsEarned = totalPoints,
        ////        Data = logsList
        ////    });
        ////}


        //// GET: api/v1/task-logs/history/{childId}?days=30
        //// Parent/Admin يشوفوا تاريخ طفل معين
        //// Child يشوف تاريخه هو بس

        //[HttpGet("history/{childId:int}")]
        //[Authorize(Roles = "Admin,Parent,Child")]
        //public async Task<IActionResult> GetHistory(int childId, [FromQuery] int days = 30)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        //    if (User.IsInRole("Child") && userId != childId)
        //        return Forbid();

        //    if (User.IsInRole("Parent"))
        //    {
        //        var isOwner = await _serviceManager.ChildService
        //            .IsChildBelongsToParentAsync(childId, userId);
        //        if (!isOwner) return Forbid();
        //    }

        //    var history = await _serviceManager.TaskLogService.GetChildHistoryAsync(childId, days);
        //    var historyList = history.ToList();

        //    return Ok(new
        //    {
        //        Success = true,
        //        Message = $"Task history for last {days} days",
        //        TotalPointsEarned = historyList.Sum(l => l.PointsEarned),
        //        TotalTasksCompleted = historyList.Count,
        //        Data = historyList
        //    });
        //}

    }
}
