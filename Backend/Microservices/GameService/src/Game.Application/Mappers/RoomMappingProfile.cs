using AutoMapper;
using Game.Application.DTOs;
using Game.Domain.Entities;

namespace Game.Application.Mappers
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<RoomDTO, Room>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tipe, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RoomStatus))
                .ForMember(dest => dest.RoundNum, opt => opt.MapFrom(src => src.RoomStatus))
                .ReverseMap();
        }
    }
}
