using AutoMapper;
using kidsApp.Application.DTOs.ChildDTOs;
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
        }
    }


}
