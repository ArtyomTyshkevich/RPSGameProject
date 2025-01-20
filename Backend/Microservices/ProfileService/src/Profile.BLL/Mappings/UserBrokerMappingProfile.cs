using Profile.BLL.DTOs;
using Profile.DAL.Entities;
using RPSGame.Broker.Events;

namespace Profile.BLL.Mappings
{
    public class UserBrokerMappingProfile : AutoMapper.Profile
    {
        public UserBrokerMappingProfile()
        {
            CreateMap<User, UserUpdatedEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Mail));

            CreateMap<UserUpdatedEvent, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(src => src.Mail));
        }
    }
}
