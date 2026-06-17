using AutoMapper;
using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TaskDTO>> GetAllAsync()
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }

        public async Task<TaskDTO?> GetByIdAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskDTO>(task);
        }

 
        public async Task<TaskDTO> CreateAsync(CreateTaskDTO dto)
        {
            var task = _mapper.Map<Tasks>(dto);
            task.TaskType = "Daily";          // Admin tasks are always Daily
            task.CreatedByChildId = null;

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDTO dto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _mapper.Map(dto, task);
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
 
        public async Task<TaskDTO> CreatePersonalTaskAsync(CreatePersonalTaskDTO dto, int childId)
        {
            var task = _mapper.Map<Tasks>(dto);
            task.TaskType = "Personal";
            task.CreatedByChildId = childId;

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

 
        public async Task<IEnumerable<TaskDTO>> GetPersonalTasksByChildIdAsync(int childId)
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            var personal = tasks
                .Where(t => t.TaskType == "Personal" && t.CreatedByChildId == childId)
                .ToList();

            return _mapper.Map<IEnumerable<TaskDTO>>(personal);
        }
 
        public async Task<IEnumerable<TaskDTO>> GetDailyTasksAsync()
        {
            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            var daily = tasks
                .Where(t => t.TaskType == "Daily")
                .ToList();

            return _mapper.Map<IEnumerable<TaskDTO>>(daily);
        }


        //public async Task<IEnumerable<ChildTaskViewDTO>> GetTodayTasksForChildAsync(int childId)
        //{
        //    var allTasks = await _unitOfWork.Tasks.GetAllAsync();

        //    var todayLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);

        //    var completedTodayIds = todayLogs
        //        .Where(l => l.Status == "Completed"
        //                 && l.DateCompleted.HasValue
        //                 && l.DateCompleted.Value.Date == DateTime.UtcNow.Date)
        //        .Select(l => l.TaskId)
        //        .ToHashSet();

        //    var visibleTasks = allTasks
        //        .Where(t =>
        //            t.TaskType == "Daily" ||
        //            // Personal: تظهر لو هي بتاعته وملقتهاش معمولة النهارده
        //            (t.TaskType == "Personal"
        //             && t.CreatedByChildId == childId
        //             && !completedTodayIds.Contains(t.Id)))
        //        .ToList();

        //    var result = visibleTasks.Select(t => new ChildTaskViewDTO
        //    {
        //        TaskId = t.Id,
        //        Title = t.Title,
        //        Description = t.Description,
        //        Category = t.Category,
        //        Difficulty = t.Difficulty,
        //        Duration = t.Duration,
        //        VideoUrl = t.VideoUrl,
        //        PointsRewarded = t.PointsRewarded,
        //        TaskType = t.TaskType,
        //        IsCompletedToday = completedTodayIds.Contains(t.Id) 
        //    });

        //    return result;
        //}

        //public async Task<IEnumerable<ChildTaskViewDTO>> GetTodayTasksForChildAsync(int childId)
        //{
        //    var allTasks = await _unitOfWork.Tasks.GetAllAsync();

        //    var todayLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);

        //    var completedTodayIds = todayLogs
        //        .Where(l => l.Status == "Completed"
        //                 && !l.IsArchived                        
        //                 && l.DateCompleted.HasValue
        //                 && l.DateCompleted.Value.Date == DateTime.UtcNow.Date)
        //        .Select(l => l.TaskId)
        //        .ToHashSet();

        //    var visibleTasks = allTasks
        //        .Where(t => t.TaskType == "Daily" ||
        //                   (t.TaskType == "Personal" && t.CreatedByChildId == childId))
        //        .ToList();

        //    return visibleTasks.Select(t => new ChildTaskViewDTO
        //    {
        //        TaskId = t.Id,
        //        Title = t.Title,
        //        Description = t.Description,
        //        Category = t.Category,
        //        Difficulty = t.Difficulty,
        //        Duration = t.Duration,
        //        VideoUrl = t.VideoUrl,
        //        PointsRewarded = t.PointsRewarded,
        //        TaskType = t.TaskType,
        //        IsCompletedToday = completedTodayIds.Contains(t.Id)
        //    });
        //}

        public async Task<IEnumerable<ChildTaskViewDTO>> GetTodayTasksForChildAsync(int childId)
        {
            var allTasks = await _unitOfWork.Tasks.GetAllAsync();

            // استخدام توقيت القاهرة لتحديد "اليوم"
            var cairoZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var todayCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoZone).Date;

            var todayLogs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);

            var completedTodayIds = todayLogs
                .Where(l => l.Status == "Completed"
                         && !l.IsArchived
                         && l.DateCompleted.HasValue
                         && l.DateCompleted.Value.Date == todayCairo)
                .Select(l => l.TaskId)
                .ToHashSet();

            var visibleTasks = allTasks
                .Where(t => t.TaskType == "Daily" ||
                           (t.TaskType == "Personal" && t.CreatedByChildId == childId))
                .ToList();

            return visibleTasks.Select(t => new ChildTaskViewDTO
            {
                TaskId = t.Id,
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Difficulty = t.Difficulty,
                Duration = t.Duration,
                VideoUrl = t.VideoUrl,
                PointsRewarded = t.PointsRewarded,
                TaskType = t.TaskType,
                IsCompletedToday = completedTodayIds.Contains(t.Id)
            });
        }
        public async Task<IEnumerable<TaskDTO>> GetTasksByDifficultyAsync(string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                return Enumerable.Empty<TaskDTO>();

            var tasks = await _unitOfWork.Tasks.GetAllAsync();

            var filtered = tasks
                .Where(t => t.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IEnumerable<TaskDTO>>(filtered);
        }
    }
}