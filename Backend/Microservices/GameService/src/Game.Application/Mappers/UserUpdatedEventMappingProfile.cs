using AutoMapper;
using Broker.Events;
using Game.Application.DTOs;
using Game.Domain.Entities;

namespace Game.Application.Mappers
{
    public class UserUpdatedEventMappingProfile : Profile
    {
        public UserUpdatedEventMappingProfile()
        {
            
            CreateMap<UserUpdatedEvent, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ReverseMap();
        }
    }
}
