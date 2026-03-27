using System;
using System.Threading;
using System.Threading.Tasks;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.Repositories;

namespace kidsApp.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KidsAppDbContext _context;

        private ChildRepository? _childRepository;
        private ParentRepository? _parentRepository;
        private GameScoreRepository? _GameScoreRepository;
        private StoryProgressRepository? _storyProgressRepository;
        private TaskRepository? _taskRepository;
        private VideoRepository? _videoRepository;
        private VideoActivityRepository? _videoActivityRepository;
        private TaskLogRepository? _taskLogRepository;




        public UnitOfWork(KidsAppDbContext context)
        {
            _context = context;
        }

        public IChildRepository Children => _childRepository ??= new ChildRepository(_context);
        public IParentRepository Parents => _parentRepository ??= new ParentRepository(_context);
        public IStoryProgressRepository StoryProgress
           => _storyProgressRepository ??= new StoryProgressRepository(_context);
        public ITaskRepository Tasks
            => _taskRepository ??= new TaskRepository(_context);
        public IVideoActivityRepository VideoActivitiesRepo
            => _videoActivityRepository ??= new VideoActivityRepository(_context);

        public IGenericRepository<Story> Stories => new GenericRepository<Story>(_context);
        public IGenericRepository<Game> Games => new GenericRepository<Game>(_context);
        public IGenericRepository<VideoActivity> VideoActivities => new GenericRepository<VideoActivity>(_context);
        public IGenericRepository<Report> Reports => new GenericRepository<Report>(_context);
        public IGameScoreRepository GameScores => _GameScoreRepository ??= new GameScoreRepository(_context);

        public IVideoRepository Videos => _videoRepository ??= new VideoRepository(_context);

        public ITaskLogRepository TaskLogs => _taskLogRepository ??= new TaskLogRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}