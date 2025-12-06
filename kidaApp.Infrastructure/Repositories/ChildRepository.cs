using Caliburn.Micro;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Domain.Entites;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class ChildRepository : GenericRepository<Child>, IChildRepository
    {
        private readonly DbSet<Child> _dbSet; // Ensure _dbSet is accessible  

        public ChildRepository(KidsAppDbContext context) : base(context)
        {
            _dbSet = context.Set<Child>(); // Initialize _dbSet  
        }

        public async Task<IEnumerable<Child>> GetChildrenByParentId(int parentId)
        {
            return await _dbSet.Where(c => c.ParentId == parentId).ToListAsync();
        }
    }
}
