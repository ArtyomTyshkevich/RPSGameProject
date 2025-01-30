using Broker.Events;
using Profile.DAL.Entities.Mongo;
namespace Profile.BLL.Mappings
{
    public class RoundResultProfile : AutoMapper.Profile
    {
        public RoundResultProfile()
        {
            CreateMap<Round, RoundResultDto>()
                .ForMember(dest => dest.RoundNumber, opt => opt.MapFrom(src => src.RoundNumber))
                .ForMember(dest => dest.FirstPlayerMove, opt => opt.MapFrom(src => src.FirstPlayerMove))
                .ForMember(dest => dest.SecondPlayerMove, opt => opt.MapFrom(src => src.SecondPlayerMove))
                .ForMember(dest => dest.RoundResult, opt => opt.MapFrom(src => src.RoundResult));

            CreateMap<RoundResultDto, Round>()
                .ForMember(dest => dest.RoundNumber, opt => opt.MapFrom(src => src.RoundNumber))
                .ForMember(dest => dest.FirstPlayerMove, opt => opt.MapFrom(src => src.FirstPlayerMove))
                .ForMember(dest => dest.SecondPlayerMove, opt => opt.MapFrom(src => src.SecondPlayerMove))
                .ForMember(dest => dest.RoundResult, opt => opt.MapFrom(src => src.RoundResult));
        }
    }
}
