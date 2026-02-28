using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class StoryProgressRepository : GenericRepository<StoryProgress>, IStoryProgressRepository
    {
        private readonly KidsAppDbContext _context;

        public StoryProgressRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoryProgress>> GetAllWithDetailsAsync()
        {
            return await _context.StoryProgress
                .Include(x => x.Child)
                .Include(x => x.Story)
                .ToListAsync();
        }

        public async Task<StoryProgress?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.StoryProgress
                .Include(x => x.Child)
                .Include(x => x.Story)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}