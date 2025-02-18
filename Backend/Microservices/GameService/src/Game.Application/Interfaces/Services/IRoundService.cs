using Game.Application.DTOs;
using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Services
{
    public interface IRoundService
    {
        Task<Message?> ProcessRound(Room room, Guid playerId, PlayerMoves move, CancellationToken cancellationToken = default);
        Task<Message?> DisconetedAsync(Room room, Guid playerId, CancellationToken cancellationToken = default);
    }
}