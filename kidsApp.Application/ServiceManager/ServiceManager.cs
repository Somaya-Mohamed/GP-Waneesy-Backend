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

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ChildService = new Lazy<IChildService>(() => new ChildService(unitOfWork, mapper));
            _ParentService = new Lazy<IParentService>(() => new ParentService(unitOfWork, mapper));
            _GameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
            _GameScoreService = new Lazy<IGameScoreService>(() => new GameScoreService(unitOfWork, mapper));
        }

        public IChildService ChildService => _ChildService.Value;

        public IParentService ParentService => _ParentService.Value;
        public IGameService GameService => _GameService.Value;
        public IGameScoreService GameScoreService => _GameScoreService.Value;


        public IStoryService StoryService => throw new NotImplementedException();

        public IStoryProgressService StoryProgressService => throw new NotImplementedException();

        public ITaskService TaskService => throw new NotImplementedException();

        public ITaskLogService TaskLogService => throw new NotImplementedException();

        public IVideoService VideoService => throw new NotImplementedException();

        public IVideoActivityService VideoActivityService => throw new NotImplementedException();
    }
}

