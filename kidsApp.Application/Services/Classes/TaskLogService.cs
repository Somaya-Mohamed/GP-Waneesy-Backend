using AutoMapper;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

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

        public async Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto)
        {
            var log = _mapper.Map<TaskLog>(dto);
            log.Status = dto.IsCompleted ? "Completed" : "Pending";
            log.DateCompleted = dto.IsCompleted ? DateTime.UtcNow : null;

            var task = await _unitOfWork.Tasks.GetByIdAsync(dto.TaskId);
            log.PointsEarned = (dto.IsCompleted && task != null) ? task.PointsRewarded : 0;

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

        public async Task<IEnumerable<TaskLogDTO>> GetTaskLogsByTaskIdAsync(int taskId)
        {
            var logs = await _unitOfWork.TaskLogs.GetByTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<TaskLogDTO>>(logs);
        }

        public async Task<IEnumerable<TaskLogDTO>> GetTaskLogsByChildIdAsync(int childId)
        {
            var logs = await _unitOfWork.TaskLogs.GetByChildIdAsync(childId);
            return _mapper.Map<IEnumerable<TaskLogDTO>>(logs);
        }
    }
}