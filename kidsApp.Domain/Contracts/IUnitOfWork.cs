using System;
using System.Threading;
using System.Threading.Tasks;
using kidsApp.Domain.Entities;

namespace kidsApp.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IChildRepository Children { get; }
        IParentRepository Parents { get; }

        IStoryProgressRepository StoryProgress { get; }
        IGenericRepository<Story> Stories { get; } // Only for Story
        IGenericRepository<Video> Videos { get; } // Videos
        IGenericRepository<Game> Games { get; } // Games
        ITaskRepository Tasks { get; }
        //IGenericRepository<Tasks> Tasks { get; } // Tasks
        //IGenericRepository<StoryProgress> StoryProgress { get; } // Story progress
        IGenericRepository<VideoActivity> VideoActivities { get; } // Video activity
        IGameScoreRepository GameScores { get; } // Game scores
        IGenericRepository<TaskLog> TaskLogs { get; } // Task logs
        IGenericRepository<Report> Reports { get; } // Reports

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}