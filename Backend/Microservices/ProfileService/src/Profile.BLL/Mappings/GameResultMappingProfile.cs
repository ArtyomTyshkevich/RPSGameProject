﻿using Profile.DAL.Entities.Mongo;
using Profile.DAL.Events;

namespace Profile.BLL.Mappings
{
    public class GameResultMappingProfile : AutoMapper.Profile
    {
        public GameResultMappingProfile()
        {
            CreateMap<Game, GameResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));

            CreateMap<GameResultDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));
        }
    }
}
