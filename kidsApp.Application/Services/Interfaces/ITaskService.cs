using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.DTOs.TaskLogDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<TaskDTO> GetByIdAsync(int id);
        Task<TaskDTO> CreateAsync(CreateTaskDTO dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDTO dto);
        Task<bool> DeleteAsync(int id);

        // Advanced Methods
        Task<IEnumerable<TaskLogDTO>> GetTaskLogsByTaskIdAsync(int taskId);
        Task<IEnumerable<TaskDTO>> GetTasksByDifficultyAsync(string level);
    }
}
