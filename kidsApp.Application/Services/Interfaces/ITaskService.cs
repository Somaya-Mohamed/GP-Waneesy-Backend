using kidsApp.Application.DTOs.TaskDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface ITaskService
    {

        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<TaskDTO?> GetByIdAsync(int id);
        Task<TaskDTO> CreateAsync(CreateTaskDTO dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<TaskDTO>> GetTasksByDifficultyAsync(string difficulty);
 
        Task<IEnumerable<TaskDTO>> GetDailyTasksAsync();

 
        Task<TaskDTO> CreatePersonalTaskAsync(CreatePersonalTaskDTO dto, int childId);
        Task<IEnumerable<TaskDTO>> GetPersonalTasksByChildIdAsync(int childId);

 
        Task<IEnumerable<ChildTaskViewDTO>> GetTodayTasksForChildAsync(int childId);
    }
}