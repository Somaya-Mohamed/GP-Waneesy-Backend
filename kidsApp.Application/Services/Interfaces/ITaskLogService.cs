using kidsApp.Application.DTOs.TaskLogDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface ITaskLogService
    {

        Task<IEnumerable<TaskLogDTO>> GetAllAsync();
        Task<TaskLogDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TaskLogDTO>> GetTaskLogsByTaskIdAsync(int taskId);
        Task<IEnumerable<TaskLogDTO>> GetTaskLogsByChildIdAsync(int childId);


        Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskLogDTO>> GetChildHistoryAsync(int childId, int days = 30);

        Task ResetDailyLogsAsync();
    }
}