using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class TaskLogRepository : GenericRepository<TaskLog>, ITaskLogRepository
    {
        public TaskLogRepository(KidsAppDbContext context) : base(context) { }

        public async Task<IEnumerable<TaskLog>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .ToListAsync();
        }

        public async Task<TaskLog?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .FirstOrDefaultAsync(tl => tl.Id == id);
        }

        public async Task<IEnumerable<TaskLog>> GetByTaskIdAsync(int taskId)
        {
            return await _dbSet
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .Where(tl => tl.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskLog>> GetByChildIdAsync(int childId)
        {
            return await _dbSet
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .Where(tl => tl.ChildId == childId)
                .OrderByDescending(tl => tl.DateCompleted)
                .ToListAsync();
        }
    }
}