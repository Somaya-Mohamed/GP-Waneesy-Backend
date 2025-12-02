using kidsApp.Application.Dto.TaskLogDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Services
{
    public interface ITaskLogService
    {
        Task<IEnumerable<TaskLogDTO>> GetAllAsync();
        Task<TaskLogDTO> GetByIdAsync(int id);
        Task<TaskLogDTO> CreateAsync(CreateTaskLogDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
