using Caliburn.Micro;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{


    public class ChildRepository : GenericRepository<Child>, IChildRepository
    {
        public ChildRepository(KidsAppDbContext context) : base(context) { }



        public async Task<IReadOnlyList<Child>> GetChildrenByParentIdAsync(int parentId)
            => await _dbSet.Where(c => c.ParentId == parentId).ToListAsync();


        public async Task<Child?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.GameScores)
                    .ThenInclude(gs => gs.Game)        
                .Include(c => c.StoryProgress)
                    .ThenInclude(sp => sp.Story)
                .Include(c => c.VideoActivities)
                    .ThenInclude(va => va.Video)
                .Include(c => c.TaskLogs)
                    .ThenInclude(tl => tl.Task)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}



//public class ChildRepository : GenericRepository<Child>, IChildRepository
//{
//    private readonly DbSet<Child> _dbSet; // Ensure _dbSet is accessible  

//    public ChildRepository(KidsAppDbContext context) : base(context)
//    {
//        _dbSet = context.Set<Child>(); // Initialize _dbSet  
//    }

//    public Task<IReadOnlyList<Child>> GetChildrenByParentIdAsync(int parentId)
//    {
//        return await _dbSet.Where(c => c.ParentId == parentId).ToListAsync();
//    }
//}