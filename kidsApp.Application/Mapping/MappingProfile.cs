using AutoMapper;
using kidsApp.Application.DTOs.ActivityDTOs;
using kidsApp.Application.DTOs.AdminDTOs;
using kidsApp.Application.DTOs.ArticleDTOs;
using kidsApp.Application.DTOs.ChildDTOs;
using kidsApp.Application.DTOs.GameDTOs;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.DTOs.ProgressDTOs;
using kidsApp.Application.DTOs.StoryDTOs;
using kidsApp.Application.DTOs.StoryProgress_DTOs;
using kidsApp.Application.DTOs.TaskDTOs;
using kidsApp.Application.DTOs.TaskLogDTOs;
using kidsApp.Application.DTOs.VideoActivityDTOs;
using kidsApp.Application.DTOs.VideoDTOs;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kidsApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // ====================== Child ======================
            CreateMap<Child, ChildReadDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => src.Preferences));

            CreateMap<ChildCreateDTO, Child>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Gender, opt => opt.Ignore())
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))           
                .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => src.Preferences)); 

            CreateMap<ChildUpdateDto, Child>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))                 
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => src.Preferences))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


            // ChildReportDTO 
            CreateMap<Child, ChildReportDTO>()
                .ForMember(dest => dest.ChildId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TotalPoints, opt => opt.Ignore())
                .ForMember(dest => dest.GamesPlayed, opt => opt.Ignore())
                .ForMember(dest => dest.StoriesCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.TasksCompleted, opt => opt.Ignore());

            // ChildTopScoreDTO 
            CreateMap<GameScore, ChildTopScoreDTO>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
                .ForMember(dest => dest.GameTitle, opt => opt.MapFrom(src => src.Game.Title))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.ScoreValue))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

            // ====================== Parent ======================
            CreateMap<Parent, ParentReadDto>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));

            CreateMap<ParentCreateDto, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateParentDTO, Parent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Parent, ParentLoginDTO>();

            CreateMap<Child, ChildSummaryDTO>()
                .ForMember(dest => dest.ChildId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

            // ====================== Game ======================
            CreateMap<GameCreateDTO, Game>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.Difficulty));

            CreateMap<GameUpdateDTO, Game>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.Difficulty))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Game, GameReadDto>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.GameLink, opt => opt.MapFrom(src => src.GameLink))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.DifficultyLevel))
                .ForMember(dest => dest.PointsRewarded, opt => opt.MapFrom(src => src.PointsRewarded));

            // ====================== GameScore ======================
            CreateMap<GameScore, GameScoreDTO>()
                .ForMember(d => d.ScoreId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Score, o => o.MapFrom(s => s.ScoreValue))
                .ForMember(d => d.ChildName, o => o.MapFrom(s => s.Child.Name))
                .ForMember(d => d.GameTitle, o => o.MapFrom(s => s.Game.Title))
                .ForMember(d => d.Attempts, o => o.MapFrom(s => s.Attempts))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date));

            CreateMap<GameScoreCreateDTO, GameScore>()
                .ForMember(dest => dest.ScoreValue, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Attempts, opt => opt.MapFrom(src => src.Attempts))
                .ForMember(dest => dest.Child, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore());

            // ====================== GameScore Create ======================
            CreateMap<GameScoreCreateDTO, GameScore>()
                .ForMember(d => d.ScoreValue, o => o.MapFrom(s => s.Score))
                .ForMember(d => d.GameId, o => o.MapFrom(s => s.GameId))
                .ForMember(d => d.ChildId, o => o.MapFrom(s => s.ChildId))
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Game, o => o.Ignore())
                .ForMember(d => d.Child, o => o.Ignore())
                .ForMember(d => d.Date, o => o.Ignore());

            // ====================== Story ======================
            CreateMap<CreateStoryDTO, Story>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.StoryText))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.AudioUrl))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateStoryDTO, Story>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.StoryText))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.AudioUrl))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Story, StoryDTO>()
                .ForMember(dest => dest.StoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StoryText, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.AudioUrl))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url));

            // ====================== StoryProgress ======================
            CreateMap<StoryProgress, StoryProgressDTO>()
                .ForMember(dest => dest.ProgressId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child != null ? src.Child.Name : "Unknown Child"))
                .ForMember(dest => dest.StoryTitle, opt => opt.MapFrom(src => src.Story != null ? src.Story.Title : "Unknown Story"))
                .ForMember(dest => dest.ProgressPercent, opt => opt.MapFrom(src => src.ProgressPercent))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated));

            CreateMap<CreateStoryProgressDTO, StoryProgress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DateCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore());

            // ====================== Task ======================
            CreateMap<CreateTaskDTO, Tasks>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateTaskDTO, Tasks>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Tasks, TaskDTO>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id));

            // ====================== TaskLog ======================
            CreateMap<TaskLog, TaskLogDTO>()
                .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child != null ? src.Child.Name : "Unknown Child"))
                .ForMember(dest => dest.TaskTitle, opt => opt.MapFrom(src => src.Task != null ? src.Task.Title : "Unknown Task"))
                .ForMember(dest => dest.PointsEarned, opt => opt.MapFrom(src => src.PointsEarned))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DateCompleted, opt => opt.MapFrom(src => src.DateCompleted));

            CreateMap<CreateTaskLogDTO, TaskLog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PointsEarned, opt => opt.Ignore())   
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DateCompleted, opt => opt.Ignore());


            // ====================== Video ======================
            CreateMap<CreateVideoDTO, Video>()
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateVideoDTO, Video>()
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.Url))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.VideoUrl))
                .ForMember(dest => dest.PointsRewarded, opt => opt.MapFrom(src => src.PointsRewarded));

            // ====================== VideoActivity ======================
            CreateMap<VideoActivity, VideoActivityDTO>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child != null ? src.Child.Name : "Unknown Child"))
                .ForMember(dest => dest.VideoTitle, opt => opt.MapFrom(src => src.Video != null ? src.Video.Title : "Unknown Video"))
                .ForMember(dest => dest.WatchPercent, opt => opt.MapFrom(src => src.WatchedPercent))  
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated ?? DateTime.UtcNow));

            CreateMap<CreateVideoActivityDTO, VideoActivity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "In Progress"))
                .ForMember(dest => dest.WatchedPercent, opt => opt.MapFrom(src => src.WatchPercent))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ====================== Article ======================


            // Create
            CreateMap<CreateArticleDTO, Article>();

            // Update
            CreateMap<UpdateArticleDTO, Article>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Read
            CreateMap<Article, ArticleDTO>();

            // ===== Admin User =====
            CreateMap<ApplicationUser, AdminUserDTO>();
            CreateMap<AdminCreateUserDTO, ApplicationUser>();
            CreateMap<AdminUpdateUserDTO, ApplicationUser>();

            // ===== Admin Role =====
            CreateMap<IdentityRole, AdminRoleDTO>();



        }
    }


}
