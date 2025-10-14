using Broker.Events;
using Profile.DAL.Entities.Mongo;
namespace Profile.BLL.Mappings
{
    public class GameResultMappingProfile : AutoMapper.Profile
    {
        public GameResultMappingProfile()
        {
            CreateMap<Game, GameResultDto>()
                 .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds ?? new List<Round>()))
                 .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId ?? string.Empty))
                 .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId ?? string.Empty))
                 .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult ?? string.Empty));


            CreateMap<GameResultDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayerId))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayerId))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));
        }
    }
}
