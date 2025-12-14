using System.Threading.Tasks;
using kidsApp.Domain.Entities;

namespace kidsApp.Domain.Contracts
{

    public interface IUnitOfWork : IDisposable
    {
        IChildRepository Children { get; }
        IParentRepository Parents { get; }

        IGenericRepository<Story> Stories { get; }
        IGenericRepository<Video> Videos { get; }
        IGenericRepository<Game> Games { get; }
        IGenericRepository<Tasks> Tasks { get; }
        IGenericRepository<StoryProgress> StoryProgress { get; }
        IGenericRepository<VideoActivity> VideoActivities { get; }
        IGenericRepository<GameScore> GameScores { get; }
        IGenericRepository<TaskLog> TaskLogs { get; }
        IGenericRepository<Report> Reports { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}


