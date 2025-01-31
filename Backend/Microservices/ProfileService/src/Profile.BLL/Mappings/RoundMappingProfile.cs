using Profile.BLL.DTOs;
using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Mappings
{
    public class RoundMappingProfile : AutoMapper.Profile
    {
        public RoundMappingProfile()
        {
            CreateMap<Round, RoundDTO>()
                .ForMember(dest => dest.RoundNumber, opt => opt.MapFrom(src => src.RoundNumber))
                .ForMember(dest => dest.FirstPlayerMove, opt => opt.MapFrom(src => src.FirstPlayerMove))
                .ForMember(dest => dest.SecondPlayerMove, opt => opt.MapFrom(src => src.SecondPlayerMove))
                .ForMember(dest => dest.RoundResult, opt => opt.MapFrom(src => src.RoundResult));

            CreateMap<RoundDTO, Round>()
                .ForMember(dest => dest.RoundNumber, opt => opt.MapFrom(src => src.RoundNumber))
                .ForMember(dest => dest.FirstPlayerMove, opt => opt.MapFrom(src => src.FirstPlayerMove))
                .ForMember(dest => dest.SecondPlayerMove, opt => opt.MapFrom(src => src.SecondPlayerMove))
                .ForMember(dest => dest.RoundResult, opt => opt.MapFrom(src => src.RoundResult));
        }
    }
}
