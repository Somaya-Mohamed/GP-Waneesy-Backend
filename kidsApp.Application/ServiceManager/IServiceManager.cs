using kidsApp.Application.Services.Interfaces;

public interface IServiceManager
{
    IChildService ChildService { get; }
    IParentService ParentService { get; }
    IGameService GameService { get; }
    IGameScoreService GameScoreService { get; }
    IStoryService StoryService { get; }
    IStoryProgressService StoryProgressService { get; }
    ITaskService TaskService { get; }
    ITaskLogService TaskLogService { get; }
    IVideoService VideoService { get; }
    IVideoActivityService VideoActivityService { get; }
    IAdminService AdminService { get; } // ← added
}
