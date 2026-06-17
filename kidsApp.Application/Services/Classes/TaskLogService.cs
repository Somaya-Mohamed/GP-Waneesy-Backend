using AutoMapper;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class TaskLogService : ITaskLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskLogDTO>> GetAllAsync()
        {
            var logs = await _unitOfWork.TaskLogs.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<TaskLogDTO>>(logs);
        }

        public async Task<TaskLogDTO?> GetByIdAsync(int id)
        {
            var log = await _unitOfWork.TaskLogs.GetByIdWithDetailsAsync(id);
            return log == null ? null : _mapper.Map<TaskLogDTO>(log);
        }

        public async Task<IEnumerable<TaskLogDTO>> GetTaskLogsByTaskIdAsync(int taskId)
        {
            var logs = await _unitOfWork.TaskLogs.GetByTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<TaskLogDTO>>(logs);
        }

        public async Task<IEnumerable<TaskLogDTO>> GetTaskLogsByChildIdAsync(int childId)
        {
            var logs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);
            return _mapper.Map<IEnumerable<TaskLogDTO>>(
                logs.Where(l => !l.IsArchived));
        }


        //  Create (child marks a task as done or pending)

        /// <summary>
        /// Creates a task log for a child.
        /// If IsCompleted = true  → status "Completed", points awarded immediately.
        /// If IsCompleted = false → status "Pending",   points = 0 (can complete later).
        /// A child cannot log the same task twice on the same day.
        /// </summary>
        //public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        //{
        //    // Guard: prevent duplicate log for same task on same day
        //    var existingLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(dto.ChildId);
        //    bool alreadyLoggedToday = existingLogs.Any(l =>
        //        l.TaskId == dto.TaskId &&
        //        l.DateCompleted.HasValue &&
        //        l.DateCompleted.Value.Date == DateTime.UtcNow.Date);

        //    if (alreadyLoggedToday)
        //        throw new InvalidOperationException("Task already completed today.");

        //    var log = _mapper.Map<TaskLog>(dto);
        //    log.Status = dto.IsCompleted ? "Completed" : "Pending";
        //    log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : null;

        //    var task = await _unitOfWork.Tasks.GetByIdAsync(dto.TaskId);
        //    log.PointsEarned = (dto.IsCompleted && task != null) ? task.PointsRewarded : 0;

        //    await _unitOfWork.TaskLogs.AddAsync(log);
        //    await _unitOfWork.SaveChangesAsync();

        //    var created = await _unitOfWork.TaskLogs.GetByIdWithDetailsAsync(log.Id);
        //    return _mapper.Map<TaskLogDTO>(created!);
        //}

        //public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        //{
        //    var existingLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(dto.ChildId);
        //    bool alreadyLoggedToday = existingLogs.Any(l =>
        //        l.TaskId == dto.TaskId &&
        //        l.DateCompleted.HasValue &&
        //        l.DateCompleted.Value.Date == DateTime.UtcNow.Date);

        //    if (alreadyLoggedToday)
        //        throw new InvalidOperationException("Task already completed today.");

        //    var task = await _unitOfWork.Tasks.GetByIdAsync(dto.TaskId);

        //    var log = _mapper.Map<TaskLog>(dto);
        //    log.Status = dto.IsCompleted ? "Completed" : "Pending";
        //    log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : null;
        //    log.PointsEarned = (dto.IsCompleted && task != null) ? task.PointsRewarded : 0;

        //    log.SnapshotTaskTitle = task?.Title ?? string.Empty;
        //    log.SnapshotTaskType = task?.TaskType ?? string.Empty;

        //    await _unitOfWork.TaskLogs.AddAsync(log);
        //    await _unitOfWork.SaveChangesAsync();

        //    var created = await _unitOfWork.TaskLogs.GetByIdWithDetailsAsync(log.Id);
        //    return _mapper.Map<TaskLogDTO>(created!);
        //}


        public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        {
            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var todayCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone).Date;

            var existingLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(dto.ChildId);

            bool alreadyLoggedToday = existingLogs.Any(l =>
                l.TaskId == dto.TaskId &&
                l.DateCompleted.HasValue &&
                l.DateCompleted.Value.Date == todayCairo);

            if (alreadyLoggedToday)
                throw new InvalidOperationException("Task already completed today.");

            var task = await _unitOfWork.Tasks.GetByIdAsync(dto.TaskId);

            var log = _mapper.Map<TaskLog>(dto);

            log.Status = dto.IsCompleted ? "Completed" : "Pending";
            log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : null;
            log.PointsEarned = (dto.IsCompleted && task != null) ? task.PointsRewarded : 0;
            log.SnapshotTaskTitle = task?.Title ?? string.Empty;
            log.SnapshotTaskType = task?.TaskType ?? string.Empty;

            await _unitOfWork.TaskLogs.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            var created = await _unitOfWork.TaskLogs.GetByIdWithDetailsAsync(log.Id);
            return _mapper.Map<TaskLogDTO>(created!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _unitOfWork.TaskLogs.GetByIdAsync(id);
            if (log == null) return false;

            _unitOfWork.TaskLogs.Delete(log);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        //  Daily Reset (called by DailyResetService at midnight)

        /// <summary>
        /// Midnight reset logic:
        ///   1. Delete ALL Completed logs → children can redo every task the next day.
        ///   2. Keep Pending logs → they remain visible so the child can still complete them.
        /// Points are already stored on the Child entity when earned, so deleting the log
        /// does NOT subtract points already awarded — points only accumulate, never reset here.
        /// </summary>

        public async Task<IEnumerable<TaskLogDTO>> GetChildHistoryAsync(int childId, int days = 30)
        {
            var logs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);
            var cutoff = DateTime.UtcNow.AddDays(-days);

            return _mapper.Map<IEnumerable<TaskLogDTO>>(
                logs.Where(l => l.IsArchived
                             && l.DateCompleted.HasValue
                             //&& (!l.DateCompleted.HasValue
                             //|| l.DateCompleted.Value >= cutoff)));
                             && l.DateCompleted.Value >= cutoff));
        }


        //public async Task ResetDailyLogsAsync()
        //{
        //    var allLogs = await _unitOfWork.TaskLogs.GetAllWithDetailsAsync();

        //    var completedLogs = allLogs
        //        .Where(l => l.Status == "Completed" && !l.IsArchived)
        //        .ToList();

        //    var completedPersonalTaskIds = completedLogs
        //        .Where(l => l.Task != null && l.Task.TaskType == "Personal")
        //        .Select(l => l.TaskId)
        //        .Distinct()
        //        .ToList();

        //    foreach (var log in completedLogs)
        //    {
        //        log.IsArchived = true;   
        //        _unitOfWork.TaskLogs.Update(log);
        //    }

        //    foreach (var taskId in completedPersonalTaskIds)
        //    {
        //        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        //        if (task != null && task.TaskType == "Personal")
        //            _unitOfWork.Tasks.Delete(task);
        //    }

        //    var cutoff = DateTime.UtcNow.AddDays(-30);
        //    var oldArchivedLogs = allLogs
        //        .Where(l => l.IsArchived
        //                 && l.DateCompleted.HasValue
        //                 && l.DateCompleted.Value < cutoff)
        //        .ToList();

        //    foreach (var log in oldArchivedLogs)
        //        _unitOfWork.TaskLogs.Delete(log);

        //    await _unitOfWork.SaveChangesAsync();
        //}



        //public async Task ResetDailyLogsAsync()
        //{
        //    var allLogs = await _unitOfWork.TaskLogs.GetAllWithDetailsAsync();

        //    var completedLogs = allLogs
        //        .Where(l => l.Status == "Completed" && !l.IsArchived)
        //        .ToList();

        //    // ── 1. اجمع الـ Personal IDs الأول ──────────────────────────────
        //    var completedPersonalTaskIds = completedLogs
        //        .Where(l => l.Task != null && l.Task.TaskType == "Personal")
        //        .Select(l => l.TaskId)
        //        .Distinct()
        //        .ToList();

        //    // ── 2. أرشف الـ Completed logs ───────────────────────────────────
        //    foreach (var log in completedLogs)
        //    {
        //        log.IsArchived = true;
        //        _unitOfWork.TaskLogs.Update(log);
        //    }

        //    // ── 3. Save الأرشفة الأول قبل ما تمسح أي task ───────────────────
        //    await _unitOfWork.SaveChangesAsync();

        //    // ── 4. امسح الـ Personal tasks بأمان ─────────────────────────────
        //    foreach (var taskId in completedPersonalTaskIds)
        //    {
        //        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId.Value);
        //        if (task == null || task.TaskType != "Personal") continue;

        //        // امسح الـ Archived logs المرتبطة بالتاسك الأول
        //        var relatedArchivedLogs = allLogs
        //            .Where(l => l.TaskId == taskId && l.IsArchived)
        //            .ToList();

        //        foreach (var log in relatedArchivedLogs)
        //            _unitOfWork.TaskLogs.Delete(log);

        //        await _unitOfWork.SaveChangesAsync();

        //        // دلوقتي امسح التاسك بأمان
        //        _unitOfWork.Tasks.Delete(task);
        //        await _unitOfWork.SaveChangesAsync();
        //    }

        //    // ── 5. امسح الـ Archived logs الأقدم من 30 يوم ──────────────────
        //    var cutoff = DateTime.UtcNow.AddDays(-30);
        //    var oldArchivedLogs = allLogs
        //        .Where(l => l.IsArchived
        //                 && l.DateCompleted.HasValue
        //                 && l.DateCompleted.Value < cutoff)
        //        .ToList();

        //    foreach (var log in oldArchivedLogs)
        //        _unitOfWork.TaskLogs.Delete(log);

        //    // ── 6. Save الحذف ────────────────────────────────────────────────
        //    await _unitOfWork.SaveChangesAsync();
        //}

        //public async Task ResetDailyLogsAsync()
        //{
        //    var allLogs = await _unitOfWork.TaskLogs.GetAllWithDetailsAsync();

        //    var completedLogs = allLogs
        //        .Where(l => l.Status == "Completed" && !l.IsArchived)
        //        .ToList();

        //    var completedPersonalTaskIds = completedLogs
        //        .Where(l => l.Task != null && l.Task.TaskType == "Personal")
        //        .Select(l => l.TaskId)
        //        .Distinct()
        //        .ToList();

        //    foreach (var log in completedLogs)
        //    {
        //        log.IsArchived = true;
        //        _unitOfWork.TaskLogs.Update(log);
        //    }

        //    await _unitOfWork.SaveChangesAsync(); 

        //    foreach (var taskId in completedPersonalTaskIds)
        //    {
        //        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId!.Value);
        //        if (task == null || task.TaskType != "Personal") continue;

        //        _unitOfWork.Tasks.Delete(task);
        //    }
        //    await _unitOfWork.SaveChangesAsync(); 

        //    var cutoff = DateTime.UtcNow.AddDays(-30);
        //    var oldArchivedLogs = allLogs
        //        .Where(l => l.IsArchived
        //                 && l.DateCompleted.HasValue
        //                 && l.DateCompleted.Value < cutoff)
        //        .ToList();

        //    foreach (var log in oldArchivedLogs)
        //        _unitOfWork.TaskLogs.Delete(log);

        //    await _unitOfWork.SaveChangesAsync();
        //}

        public async Task ResetDailyLogsAsync()
        {
            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var nowUtc = DateTime.UtcNow;
            var todayCairo = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, cairoZone).Date;

            var allLogs = await _unitOfWork.TaskLogs.GetAllWithDetailsAsync();

            var completedLogs = allLogs
                .Where(l => l.Status == "Completed" && !l.IsArchived)
                .ToList();

            foreach (var log in completedLogs)
            {
                log.IsArchived = true;
                _unitOfWork.TaskLogs.Update(log);
            }
            await _unitOfWork.SaveChangesAsync();

            var completedPersonalTaskIds = completedLogs
                .Where(l => l.Task != null && l.Task.TaskType == "Personal")
                .Select(l => l.TaskId)
                .Distinct()
                .ToList();

            foreach (var taskId in completedPersonalTaskIds)
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(taskId!.Value);
                if (task == null || task.TaskType != "Personal") continue;

                _unitOfWork.Tasks.Delete(task);
            }
            await _unitOfWork.SaveChangesAsync();

            var cutoff = nowUtc.AddDays(-30);
            var oldArchivedLogs = allLogs
                .Where(l => l.IsArchived
                         && l.DateCompleted.HasValue
                         && l.DateCompleted.Value < cutoff)
                .ToList();

            foreach (var log in oldArchivedLogs)
                _unitOfWork.TaskLogs.Delete(log);

            await _unitOfWork.SaveChangesAsync();

            // Console.WriteLine($"Daily Reset completed at {todayCairo} | Archived: {completedLogs.Count}");
        }



    }
}