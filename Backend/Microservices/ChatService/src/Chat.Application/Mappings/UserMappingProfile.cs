using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.NickName, options => options.MapFrom(source => source.NickName));

            CreateMap<UserDTO, User>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.NickName, options => options.MapFrom(source => source.NickName))
                .ForMember(destination => destination.Email, options => options.Ignore());
        }
    }
}