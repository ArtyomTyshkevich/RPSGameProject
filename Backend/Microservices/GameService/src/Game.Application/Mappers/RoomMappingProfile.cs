using AutoMapper;
using Game.Application.DTOs;
using Game.Domain.Entities;

namespace Game.Application.Mappers
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDTO>()
             .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Tipe))
             .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<RoomDTO, Room>()
                .ForMember(dest => dest.Tipe, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RoomStatus))
                .ForMember(dest => dest.Rounds, opt => opt.Ignore())
                .ForMember(dest => dest.GameResult, opt => opt.Ignore());
        }
    }
}
