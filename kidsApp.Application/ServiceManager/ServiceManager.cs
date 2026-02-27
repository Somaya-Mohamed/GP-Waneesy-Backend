


using AutoMapper;
using kidsApp.Application.Services;
using kidsApp.Application.Services.Classes;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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

        //public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IAdminService adminService)
        public ServiceManager(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
        {
            ChildService = new ChildService(unitOfWork, mapper);
            ParentService = new ParentService(unitOfWork, mapper);
            GameService = new GameService(unitOfWork, mapper);
            GameScoreService = new GameScoreService(unitOfWork, mapper);
            VideoService = new VideoService(unitOfWork, mapper);

            StoryService = new StoryService(unitOfWork, mapper);
            StoryProgressService = new StoryProgressService(unitOfWork, mapper);
            TaskService = new TaskService(unitOfWork, mapper);
            TaskLogService = new TaskLogService(unitOfWork, mapper);
            VideoActivityService = new VideoActivityService(unitOfWork, mapper);

            AdminService = new AdminService(userManager, roleManager, mapper);
        }
    }
}

