using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class TaskLogRepository : GenericRepository<TaskLog>, IGenericRepository<TaskLog>
    {
        private readonly KidsAppDbContext _context;

        public TaskLogRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskLog>> GetAllWithRelationsAsync()
        {
            return await _context.TaskLogs
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .ToListAsync();
        }

        public async Task<TaskLog> GetByIdWithRelationsAsync(int id)
        {
            return await _context.TaskLogs
                .Include(tl => tl.Child)
                .Include(tl => tl.Task)
                .FirstOrDefaultAsync(tl => tl.Id == id);
        }
    }
}