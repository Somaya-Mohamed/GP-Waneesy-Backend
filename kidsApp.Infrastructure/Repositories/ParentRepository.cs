using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class ParentRepository : GenericRepository<Parent>, IParentRepository
    {
        private readonly DbSet<Parent> _dbSet;

        public ParentRepository(KidsAppDbContext context) : base(context)
        {
            _dbSet = context.Set<Parent>();
        }

        public async Task<Parent?> GetParentWithChildren(int parentId)
        {
            return await _dbSet
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == parentId);
        }
    }
}
