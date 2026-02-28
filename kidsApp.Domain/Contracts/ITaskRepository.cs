using kidsApp.Domain.Entities;

namespace kidsApp.Domain.Contracts
{
    public interface ITaskRepository : IGenericRepository<Tasks>
    {
        Task<IEnumerable<Tasks>> GetByCategoryAsync(string category);
        Task<Tasks?> GetWithLogsAsync(int taskId);
    }
}