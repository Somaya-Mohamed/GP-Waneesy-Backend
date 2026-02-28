using kidsApp.Application.DTOs.TaskLogDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.Interfaces
{
    public interface ITaskLogService
    {
        Task<IEnumerable<TaskLogDTO>> GetAllAsync();
        Task<TaskLogDTO> GetByIdAsync(int id);
        Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}