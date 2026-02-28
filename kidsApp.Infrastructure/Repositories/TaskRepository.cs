using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Tasks>, ITaskRepository
    {
        private readonly KidsAppDbContext _context;

        public TaskRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetByCategoryAsync(string category)
        {
            return await _context.Tasks
                .Where(t => t.Category == category)
                .ToListAsync();
        }

        public async Task<Tasks?> GetWithLogsAsync(int taskId)
        {
            return await _context.Tasks
                .Include(t => t.TaskLogs)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
    }
}