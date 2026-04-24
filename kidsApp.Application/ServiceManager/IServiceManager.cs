using kidsApp.Application.Services.Interfaces;
namespace kidsApp.Application.ServiceManager
{

    public interface IServiceManager
    {
        IChildService ChildService { get; }
        IParentService ParentService { get; }
        IGameService GameService { get; }
        IGameScoreService GameScoreService { get; }
        IStoryService StoryService { get; }
        IArticleService ArticleService { get; }
        IStoryProgressService StoryProgressService { get; }
        ITaskService TaskService { get; }
        ITaskLogService TaskLogService { get; }
        IVideoService VideoService { get; }
        IVideoActivityService VideoActivityService { get; }
        IAdminService AdminService { get; } // ← added
    }
}