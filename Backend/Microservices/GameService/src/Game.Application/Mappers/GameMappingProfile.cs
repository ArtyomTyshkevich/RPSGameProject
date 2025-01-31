using AutoMapper;
using Broker.Events;
using Game.Domain.Entities;

namespace Game.Application.Mappers
{
    public class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
            CreateMap<Room, GameResultDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.FirstPlayerId, opt => opt.MapFrom(src => src.FirstPlayer != null ? src.FirstPlayer.Id.ToString() : null))
                .ForMember(dest => dest.SecondPlayerId, opt => opt.MapFrom(src => src.SecondPlayer != null ? src.SecondPlayer.Id.ToString() : null))
                .ForMember(dest => dest.GameResult, opt => opt.MapFrom(src => src.GameResult != null ? src.GameResult.ToString() : null))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src =>
                     src.Rounds
                .Where(round => round.RoundResult != null)
                .Select((round, index) => new RoundResultDto
                {
                    RoundNumber = index + 1,
                    FirstPlayerMove = round.FirstPlayerMove.ToString(),
                    SecondPlayerMove = round.SecondPlayerMove.ToString(),
                    RoundResult = round.RoundResult.ToString()
                }).ToList()));
        }
    }
}