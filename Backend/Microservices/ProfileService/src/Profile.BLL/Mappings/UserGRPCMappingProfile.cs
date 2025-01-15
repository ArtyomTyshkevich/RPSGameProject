using Profile.BLL.DTOs;
using Profile.DAL.Entities;
using UserGrpcService;

namespace Profile.BLL.Mappings
{
    public class UserGRPCMappingProfile : AutoMapper.Profile
    {
        public UserGRPCMappingProfile()
        {
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Email))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Mail));
        }
    }
}

