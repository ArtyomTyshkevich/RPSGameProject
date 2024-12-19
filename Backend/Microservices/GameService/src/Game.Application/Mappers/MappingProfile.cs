using AutoMapper;
using Game.Application.DTOs;
using Game.Domain.Entities;

namespace Game.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomDTO, Room>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tipe, opt => opt.MapFrom(src => src.RoomTipe))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RoomStatus))
                .ForMember(dest => dest.RoundNum, opt => opt.MapFrom(src => src.RoomStatus))
                .ReverseMap();
            
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ReverseMap();
        }
    }
}
