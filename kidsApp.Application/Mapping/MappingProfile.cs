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
           /// Mapping من Parent Entity → ParentReadDto
            CreateMap<Parent, ParentReadDto>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Id)) // Id → ParentId
                .ForMember(dest => dest.Children, opt => opt.Ignore()); // لو عايز تضيف mapping للأطفال لاحقاً

            // Mapping من ParentCreateDto → Parent Entity
            CreateMap<ParentCreateDto, Parent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id هيتحدد تلقائياً من قاعدة البيانات

            // Mapping من UpdateParentDTO → Parent Entity
            CreateMap<UpdateParentDTO, Parent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Mapping من Parent → ParentLoginDTO
            CreateMap<Parent, ParentLoginDTO>();

            //CreateMap<Parent, ParentReadDto>();
            //CreateMap<ParentCreateDto, Parent>();
            //CreateMap<UpdateParentDTO, Parent>()
            //    .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            //CreateMap<Parent, ParentLoginDTO>();

            // ====================== Game ======================
            CreateMap<GameCreateDTO, Game>()
     .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.Difficulty));

            CreateMap<GameUpdateDTO, Game>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.Difficulty));

            CreateMap<Game, GameReadDto>()
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.DifficultyLevel));
            // ====================== GameScore ======================
           CreateMap<GameScore, GameScoreDTO>()
               .ForMember(d => d.ScoreId, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Score, o => o.MapFrom(s => s.ScoreValue))
               .ForMember(d => d.ChildName, o => o.MapFrom(s => s.Child.Name))
               .ForMember(d => d.GameTitle, o => o.MapFrom(s => s.Game.Title));

            // ====================== GameScore Create ======================
            CreateMap<GameScoreCreateDTO, GameScore>()
                .ForMember(d => d.ScoreValue, o => o.MapFrom(s => s.Score))
                .ForMember(d => d.GameId, o => o.MapFrom(s => s.GameId))
                .ForMember(d => d.ChildId, o => o.MapFrom(s => s.ChildId))
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Game, o => o.Ignore())
                .ForMember(d => d.Child, o => o.Ignore())
                .ForMember(d => d.Date, o => o.Ignore());
            CreateMap<CreateStoryDTO, Story>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.StoryText))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateStoryDTO, Story>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.StoryText))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Difficulty));

            CreateMap<Story, StoryDTO>()
                .ForMember(dest => dest.StoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StoryText, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Category));
            // ====================== StoryProgress ======================
            CreateMap<StoryProgress, StoryProgressDTO>();
            CreateMap<CreateStoryProgressDTO, StoryProgress>();
            CreateMap<ProgressCreateDto, StoryProgress>();

            // ====================== Task ======================
            // Mapping for creating task
            CreateMap<CreateTaskDTO, Tasks>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty));

            // Mapping for returning DTO
            CreateMap<TaskLog, TaskLogDTO>()
     .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.Id))
     .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.Name))
     .ForMember(dest => dest.TaskTitle, opt => opt.MapFrom(src => src.Task.Title));

            CreateMap<CreateTaskLogDTO, TaskLog>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DateCompleted, opt => opt.Ignore());




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
