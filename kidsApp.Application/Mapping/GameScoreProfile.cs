using AutoMapper;
using kidsApp.Application.DTOs.GameScoreDTOs;
using kidsApp.Domain.Entities;

namespace kidsApp.Application.Mapping
{
    public class GameScoreProfile : Profile
    {
        public GameScoreProfile()
        {
            // Mapping from DTO to entity (Create)
            CreateMap<GameScoreCreateDTO, GameScore>()
                .ForMember(dest => dest.ScoreValue, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.Child, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore()); // we will set Date manually

            // Mapping from entity to DTO (Read)
            CreateMap<GameScore, GameScoreDTO>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.ScoreValue))
                .ForMember(dest => dest.ScoreId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.Name))
                .ForMember(dest => dest.GameTitle, opt => opt.MapFrom(src => src.Game.Title));

            // Mapping for Update
            CreateMap<GameScoreUpdateDTO, GameScore>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}