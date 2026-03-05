using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class VideoRepository : GenericRepository<Video>, IVideoRepository
{
    private readonly KidsAppDbContext _context;

    public VideoRepository(KidsAppDbContext context) : base(context)
    {
        _context = context;
    }

    

    public async Task<IEnumerable<Video>> GetByCategoryAsync(string level)
    {
        return await _context.Videos
            .Where(v => v.Category == level)
            .ToListAsync();
    }

    public async Task<IEnumerable<Video>> GetTopWatchedAsync(int topCount)
    {
        return await _context.VideoActivities
            .GroupBy(a => a.VideoId)
            .OrderByDescending(g => g.Count())
            .Take(topCount)
            .Select(g => g.First().Video)
            .ToListAsync();
    }
}