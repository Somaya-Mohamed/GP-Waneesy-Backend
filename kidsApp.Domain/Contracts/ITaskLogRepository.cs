using kidsApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Domain.Contracts
{
    public interface ITaskLogRepository : IGenericRepository<TaskLog>
    {
        Task<IEnumerable<TaskLog>> GetAllWithDetailsAsync();
        Task<TaskLog?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<TaskLog>> GetByTaskIdAsync(int taskId);
        Task<IEnumerable<TaskLog>> GetByChildIdAsync(int childId);
    }
}