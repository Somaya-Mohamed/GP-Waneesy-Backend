using AutoMapper;
using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;

namespace kidsApp.Application.Services.Classes
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
            var task = new Tasks
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Difficulty   // 👈 mapping
            };

            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(task);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDTO dto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null) return false;

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;
            task.Category = dto.Difficulty ?? task.Category;

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

        public async Task<IEnumerable<TaskLogDTO>> GetTaskLogsByTaskIdAsync(int taskId)
        {
            var task = await _unitOfWork.Tasks.GetWithLogsAsync(taskId);
            if (task == null) return Enumerable.Empty<TaskLogDTO>();

            return _mapper.Map<IEnumerable<TaskLogDTO>>(task.TaskLogs);
        }

        public async Task<IEnumerable<TaskDTO>> GetTasksByDifficultyAsync(string difficulty)
        {
            var tasks = await _unitOfWork.Tasks.GetByCategoryAsync(difficulty);
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }
    }
}