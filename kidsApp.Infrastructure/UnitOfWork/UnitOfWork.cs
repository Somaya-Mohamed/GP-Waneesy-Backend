using kidsApp.Application.Interfaces;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Domain.Entites;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.Repositories;

namespace kidsApp.Infrastructure.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KidsAppDbContext _context;

        private ChildRepository? _childRepository;
        private ParentRepository? _parentRepository;

        public UnitOfWork(KidsAppDbContext context)
        {
            _context = context;
        }

        public IChildRepository Children => _childRepository ??= new ChildRepository(_context);
        public IParentRepository Parents => _parentRepository ??= new ParentRepository(_context);

        public IGenericRepository<Story> Stories => new GenericRepository<Story>(_context);
        public IGenericRepository<Video> Videos => new GenericRepository<Video>(_context);
        public IGenericRepository<Game> Games => new GenericRepository<Game>(_context);
        public IGenericRepository<Tasks> Tasks => new GenericRepository<Tasks>(_context);
        public IGenericRepository<StoryProgress> StoryProgress => new GenericRepository<StoryProgress>(_context);
        public IGenericRepository<VideoActivity> VideoActivities => new GenericRepository<VideoActivity>(_context);
        public IGenericRepository<GameScore> GameScores => new GenericRepository<GameScore>(_context);
        public IGenericRepository<TaskLog> TaskLogs => new GenericRepository<TaskLog>(_context);
        public IGenericRepository<Report> Reports => new GenericRepository<Report>(_context);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}


