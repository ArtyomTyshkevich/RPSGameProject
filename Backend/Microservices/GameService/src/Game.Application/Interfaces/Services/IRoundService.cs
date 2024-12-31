using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Services
{
    public interface IRoundService
    {
        Task ProcessRound(Room room, Guid playerId, PlayerMoves move, CancellationToken cancellationToken);
    }
}