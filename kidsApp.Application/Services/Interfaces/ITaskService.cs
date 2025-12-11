using kidsApp.Application.Dto.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services.S_Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<TaskDTO> GetByIdAsync(int id);
        Task<TaskDTO> CreateAsync(CreateTaskDTO dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
