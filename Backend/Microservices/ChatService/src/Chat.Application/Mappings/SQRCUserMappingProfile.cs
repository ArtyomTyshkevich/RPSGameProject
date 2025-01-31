using AutoMapper;
using Chat.Domain.Entities;
using UserGrpcService;

namespace Chat.Application.Mappings
{
    public class SQRCUserMappingProfile : Profile
    {
        public SQRCUserMappingProfile()
        {
            CreateMap<User, UserRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
