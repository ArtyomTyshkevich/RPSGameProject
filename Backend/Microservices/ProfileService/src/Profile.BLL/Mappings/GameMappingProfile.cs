using Profile.BLL.DTOs;
using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Mappings
{
    public class GameMappingProfile : AutoMapper.Profile
    {
        public GameMappingProfile()
        {
            CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));

            CreateMap<GameDTO, Game>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));
        }
    }
}
