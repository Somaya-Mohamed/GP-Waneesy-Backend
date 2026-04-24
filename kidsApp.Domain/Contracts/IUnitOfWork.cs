using System;
using System.Threading;
using System.Threading.Tasks;
using kidsApp.Domain.Entities;

namespace kidsApp.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IChildRepository Children { get; }
        IParentRepository Parents { get; }
        IGenericRepository<Article> Articles { get; }
        IStoryProgressRepository StoryProgress { get; }
        IVideoRepository Videos { get; }
        IVideoActivityRepository VideoActivitiesRepo { get; }

        ITaskRepository Tasks { get; }
        ITaskLogRepository TaskLogs { get; }           

        IGameScoreRepository GameScores { get; }

        // Generic ones
        IGenericRepository<Story> Stories { get; }
        IGenericRepository<Game> Games { get; }
        IGenericRepository<Report> Reports { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}