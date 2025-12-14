using kidsApp.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.ServiceManager
{
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
    }
}
