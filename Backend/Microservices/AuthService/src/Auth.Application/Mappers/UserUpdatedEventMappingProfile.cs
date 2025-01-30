using Auth.DAL.Entities;
using AutoMapper;
using Broker.Events;

namespace Auth.BLL.Mappers
{
    public class UserUpdatedEventMappingProfile : Profile
    {
        public UserUpdatedEventMappingProfile()
        {
            CreateMap<User, UserUpdatedEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratting));

            CreateMap<UserUpdatedEvent, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Mail))
                .ForMember(dest => dest.Ratting, opt => opt.MapFrom(src => src.Rating));
        }
    }
}
