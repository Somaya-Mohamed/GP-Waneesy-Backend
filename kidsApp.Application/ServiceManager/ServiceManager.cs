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


        private readonly Lazy<IChildService> _ChildService;
        private readonly Lazy<IParentService> _ParentService;
        private readonly Lazy<IGameService> _GameService;
        private readonly Lazy<IGameScoreService> _GameScoreService;
        private readonly Lazy<IStoryService> _StoryService;
        private readonly Lazy<IStoryProgressService> _StoryProgressService;
        private readonly Lazy<ITaskService> _TaskService;
        private readonly Lazy<ITaskLogService> _TaskLogService;
        private readonly Lazy<IVideoService> _VideoService;
        private readonly Lazy<IVideoActivityService> _VideoActivityService;
        private readonly Lazy<IAdminService> _AdminService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ChildService = new Lazy<IChildService>(() => new ChildService(unitOfWork, mapper));
            _ParentService = new Lazy<IParentService>(() => new ParentService(unitOfWork, mapper));
            _GameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
            _GameScoreService = new Lazy<IGameScoreService>(() => new GameScoreService(unitOfWork, mapper));
            //_StoryService = new Lazy<IStoryService>(() => new StoryService(unitOfWork, mapper));
            //_StoryProgressService = new Lazy<IStoryProgressService>(() => new StoryProgressService(unitOfWork, mapper));
            //_TaskService = new Lazy<ITaskService>(() => new TaskService(unitOfWork, mapper));
            //_TaskLogService = new Lazy<ITaskLogService>(() => new TaskLogService(unitOfWork, mapper));
            //_VideoService = new Lazy<IVideoService>(() => new VideoService(unitOfWork, mapper));
            //_VideoActivityService = new Lazy<IVideoActivityService>(() => new VideoActivityService(unitOfWork, mapper));

            //_AdminService = new Lazy<IAdminService>(() => new AdminService(unitOfWork, mapper)); // ← added
        }

        public IChildService ChildService => _ChildService.Value;

        public IParentService ParentService => _ParentService.Value;

        public IGameService GameService => _GameService.Value;

        public IGameScoreService GameScoreService => _GameScoreService.Value;

        //public IStoryService StoryService => throw new NotImplementedException();

        //public IStoryProgressService StoryProgressService => throw new NotImplementedException();

        //public ITaskService TaskService => throw new NotImplementedException();

        //public ITaskLogService TaskLogService => throw new NotImplementedException();

        //public IVideoService VideoService => throw new NotImplementedException();

        //public IVideoActivityService VideoActivityService => throw new NotImplementedException();

        public IStoryService StoryService => _StoryService.Value;
        public IStoryProgressService StoryProgressService => _StoryProgressService.Value;
        public ITaskService TaskService => _TaskService.Value;
        public ITaskLogService TaskLogService => _TaskLogService.Value;
        public IVideoService VideoService => _VideoService.Value;
        public IVideoActivityService VideoActivityService => _VideoActivityService.Value;
        public IAdminService AdminService => _AdminService.Value; // ← added
    }
}

