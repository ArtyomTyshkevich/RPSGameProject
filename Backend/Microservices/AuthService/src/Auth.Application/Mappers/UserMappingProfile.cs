using Auth.DAL.Entities;
using AutoMapper;
using UserGrpcService;

namespace Auth.BLL.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratting));

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Ratting, opt => opt.MapFrom(src => src.Rating));
        }
    }
}
