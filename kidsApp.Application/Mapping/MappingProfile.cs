using AutoMapper;
using kidsApp.Application.DTOs.ActivityDTOs;
using kidsApp.Application.DTOs.AdminDTOs;
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
using kidsApp.Domain.Entities;
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

            // ====================== Child Maps ======================
            CreateMap<Child, ChildReadDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));   

            CreateMap<ChildCreateDTO, Child>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Gender, opt => opt.Ignore())      
                .ForMember(dest => dest.Avatar, opt => opt.Ignore())
                .ForMember(dest => dest.Preferences, opt => opt.Ignore());

            CreateMap<ChildUpdateDto, Child>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
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
            CreateMap<Parent, ParentReadDto>();
            CreateMap<ParentCreateDto, Parent>();
            CreateMap<UpdateParentDTO, Parent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Parent, ParentLoginDTO>();

            // ====================== Game ======================
            CreateMap<Game, GameReadDto>();
            CreateMap<GameCreateDTO, Game>();
            CreateMap<GameUpdateDTO, Game>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ====================== GameScore ======================
            CreateMap<GameScore, GameScoreDTO>();
            CreateMap<GameScoreCreateDTO, GameScore>();
            CreateMap<GameScoreUpdateDTO, GameScore>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ====================== Story ======================
            CreateMap<Story, StoryDTO>();
            CreateMap<CreateStoryDTO, Story>();
            CreateMap<UpdateStoryDTO, Story>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ====================== StoryProgress ======================
            CreateMap<StoryProgress, StoryProgressDTO>();
            CreateMap<CreateStoryProgressDTO, StoryProgress>();
            CreateMap<ProgressCreateDto, StoryProgress>();

            // ====================== Task ======================
            CreateMap<Tasks, TaskDTO>();
            CreateMap<CreateTaskDTO, Tasks>();
            CreateMap<UpdateTaskDTO, Tasks>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ====================== TaskLog ======================
            CreateMap<TaskLog, TaskLogDTO>();
            CreateMap<CreateTaskLogDTO, TaskLog>();

            // ====================== Video ======================
            CreateMap<Video, VideoDTO>();
            CreateMap<CreateVideoDTO, Video>();
            CreateMap<UpdateVideoDTO, Video>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ====================== VideoActivity ======================
            CreateMap<VideoActivity, VideoActivityDTO>();
            CreateMap<CreateVideoActivityDTO, VideoActivity>();
            CreateMap<ActivityCreateDto, VideoActivity>();
            CreateMap<VideoActivityDTO, VideoActivity>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



        }
    }


}
