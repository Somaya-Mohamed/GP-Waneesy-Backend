using kidsApp.Application.Interfaces;
using kidsApp.Application.Interfaces.Repository;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.Repositories;

namespace kidsApp.Infrastructure.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KidsAppDbContext _context;

        public UnitOfWork(KidsAppDbContext context)
        {
            _context = context;

            Parents = new ParentRepository(context);
            Children = new ChildRepository(context);
            Stories = new StoryRepository(context);
            StoryProgress = new StoryProgressRepository(context);
            Videos = new VideoRepository(context);
            VideoActivities = new VideoActivityRepository(context);
            Games = new GameRepository(context);
            GameScores = new GameScoreRepository(context);
            Tasks = new TaskRepository(context);
            TaskLogs = new TaskLogRepository(context);
            Reports = new ReportRepository(context);
        }

        public IParentRepository Parents { get; }
        public IChildRepository Children { get; }
        public IStoryRepository Stories { get; }
        public IStoryProgressRepository StoryProgress { get; }
        public IVideoRepository Videos { get; }
        public IVideoActivityRepository VideoActivities { get; }
        public IGameRepository Games { get; }
        public IGameScoreRepository GameScores { get; }
        public ITaskRepository Tasks { get; }
        public ITaskLogRepository TaskLogs { get; }
        public IReportRepository Reports { get; }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
