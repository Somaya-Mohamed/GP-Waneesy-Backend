using System.Threading.Tasks;
using kidsApp.Application.Interfaces.Repository;

namespace kidsApp.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IParentRepository Parents { get; }
        IChildRepository Children { get; }
        IStoryRepository Stories { get; }
        IStoryProgressRepository StoryProgress { get; }
        IVideoRepository Videos { get; }
        IVideoActivityRepository VideoActivities { get; }
        IGameRepository Games { get; }
        IGameScoreRepository GameScores { get; }
        ITaskRepository Tasks { get; }
        ITaskLogRepository TaskLogs { get; }
        IReportRepository Reports { get; }

        Task<int> SaveAsync();
    }
}
