using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kidsApp.Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Tasks>, ITaskRepository
    {
        public TaskRepository(KidsAppDbContext context) : base(context) { }

        public async Task<IEnumerable<Tasks>> GetByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}