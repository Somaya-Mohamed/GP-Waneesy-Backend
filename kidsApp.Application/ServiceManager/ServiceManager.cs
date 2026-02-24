//using AutoMapper;
////using kidsApp.Application.Services.Classes;
//using kidsApp.Application.Services.Interfaces;
//using kidsApp.Application.Services;
//using kidsApp.Domain.Contracts;


//public class ServiceManager : IServiceManager
//{
//    private readonly Lazy<IChildService> _ChildService;
//    private readonly Lazy<IParentService> _ParentService;
//    private readonly Lazy<IGameService> _GameService;
//    private readonly Lazy<IGameScoreService> _GameScoreService;
//    private readonly Lazy<IStoryService> _StoryService;
//    private readonly Lazy<IStoryProgressService> _StoryProgressService;
//    private readonly Lazy<ITaskService> _TaskService;
//    private readonly Lazy<ITaskLogService> _TaskLogService;
//    private readonly Lazy<IVideoService> _VideoService;
//    private readonly Lazy<IVideoActivityService> _VideoActivityService;
//    private readonly Lazy<IAdminService> _AdminService;

//    public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
//    {
//        _ChildService = new Lazy<IChildService>(() => new ChildService(unitOfWork, mapper));
//        _ParentService = new Lazy<IParentService>(() => new ParentService(unitOfWork, mapper));
//        _GameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
//        _GameScoreService = new Lazy<IGameScoreService>(() => new GameScoreService(unitOfWork, mapper));

//        //_StoryService = new Lazy<IStoryService>(() => new StoryService(unitOfWork, mapper));
//        //_StoryProgressService = new Lazy<IStoryProgressService>(() => new StoryProgressService(unitOfWork, mapper));
//        //_TaskService = new Lazy<ITaskService>(() => new TaskService(unitOfWork, mapper));
//        //_TaskLogService = new Lazy<ITaskLogService>(() => new TaskLogService(unitOfWork, mapper));
//        //_VideoService = new Lazy<IVideoService>(() => new VideoService(unitOfWork, mapper));
//        //_VideoActivityService = new Lazy<IVideoActivityService>(() => new VideoActivityService(unitOfWork, mapper));

//        //_AdminService = new Lazy<IAdminService>(() => new AdminService(unitOfWork, mapper)); // ← added
//    }

//    public IChildService ChildService => _ChildService.Value;
//    public IParentService ParentService => _ParentService.Value;
//    public IGameService GameService => _GameService.Value;
//    public IGameScoreService GameScoreService => _GameScoreService.Value;
//    public IStoryService StoryService => _StoryService.Value;
//    public IStoryProgressService StoryProgressService => _StoryProgressService.Value;
//    public ITaskService TaskService => _TaskService.Value;
//    public ITaskLogService TaskLogService => _TaskLogService.Value;
//    public IVideoService VideoService => _VideoService.Value;
//    public IVideoActivityService VideoActivityService => _VideoActivityService.Value;
//    public IAdminService AdminService => _AdminService.Value; // ← added
//}

using AutoMapper;
using kidsApp.Application.Services;
using kidsApp.Application.Services.Classes;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.ServiceManager
{
    public class ServiceManager : IServiceManager
    {
        public IChildService ChildService { get; }
        public IParentService ParentService { get; }
        public IGameService GameService { get; }
        public IGameScoreService GameScoreService { get; }
        public IStoryService StoryService { get; }
        public IStoryProgressService StoryProgressService { get; }
        public ITaskService TaskService { get; }
        public ITaskLogService TaskLogService { get; }
        public IVideoService VideoService { get; }
        public IVideoActivityService VideoActivityService { get; }
        public IAdminService AdminService { get; }

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            ChildService = new ChildService(unitOfWork, mapper);
            ParentService = new ParentService(unitOfWork, mapper);
            GameService = new GameService(unitOfWork, mapper);
            GameScoreService = new GameScoreService(unitOfWork, mapper);
            StoryService = new StoryService(unitOfWork, mapper);
            StoryProgressService ??= new StoryProgressService(unitOfWork, mapper);
            TaskService = new TaskService(unitOfWork, mapper);
            TaskLogService = new TaskLogService(unitOfWork, mapper);
            //StoryProgressService = new StoryProgressService(unitOfWork, mapper);
            //TaskService = new TaskService(unitOfWork, mapper);
            //TaskLogService = new TaskLogService(unitOfWork, mapper);
            //VideoService = new VideoService(unitOfWork, mapper);
            //VideoActivityService = new VideoActivityService(unitOfWork, mapper);
            //AdminService = new AdminService(unitOfWork, mapper);
        }
    }
}

