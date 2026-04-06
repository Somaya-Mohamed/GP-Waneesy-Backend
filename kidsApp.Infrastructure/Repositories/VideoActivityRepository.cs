using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kidsApp.Infrastructure.Repositories
{
    public class VideoActivityRepository : GenericRepository<VideoActivity>, IVideoActivityRepository
    {
        private readonly KidsAppDbContext _context;

        public VideoActivityRepository(KidsAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<VideoActivity> GetByIdWithDetailsAsync(int id)
        {
            return await _context.VideoActivities
                .Include(v => v.Child)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VideoActivity>> GetAllWithDetailsAsync()
        {
            return await _context.VideoActivities
                .Include(v => v.Child)
                .Include(v => v.Video)
                .ToListAsync();
        }

        public async Task<IEnumerable<VideoActivity>> GetByChildIdWithDetailsAsync(int childId)
        {
            return await _context.VideoActivities
                .Include(v => v.Child)
                .Include(v => v.Video)
                .Where(v => v.ChildId == childId)
                .ToListAsync();
        }

        public async Task<IEnumerable<VideoActivity>> GetByVideoIdWithDetailsAsync(int videoId)
        {
            return await _context.VideoActivities
                .Include(v => v.Child)
                .Include(v => v.Video)
                .Where(v => v.VideoId == videoId)
                .ToListAsync();
        }
    }
}