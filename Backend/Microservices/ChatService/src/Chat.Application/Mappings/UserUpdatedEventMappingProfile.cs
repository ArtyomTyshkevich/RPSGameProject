using AutoMapper;
using Broker.Events;
using Chat.Domain.Entities;

namespace Chat.Application.Mappings
{
    public class UserUpdatedEventMappingProfile : Profile
    {
        public UserUpdatedEventMappingProfile()
        {
            CreateMap<User, UserUpdatedEvent>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.NickName))
                .ForMember(destination => destination.Mail, options => options.MapFrom(source => source.Email));

            CreateMap<UserUpdatedEvent, User>()
                .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.NickName, options => options.MapFrom(source => source.Name))
                .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Mail));
        }
    }
}