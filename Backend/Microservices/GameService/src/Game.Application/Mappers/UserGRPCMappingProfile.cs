using AutoMapper;
using Game.Domain.Entities;
using UserGrpcService;

namespace Game.Application.Mappers
{
    public class UserGRPCMappingProfile : Profile
    {
        public UserGRPCMappingProfile()
        {
            CreateMap<User, UserRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));
        }
    }
}
