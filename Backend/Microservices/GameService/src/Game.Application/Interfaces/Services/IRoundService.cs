using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Services
{
    public interface IRoundService
    {
        Task ProcessRound(Room room, PlayerMoves firstPlayerMove, PlayerMoves secondPlayerMove, CancellationToken cancellationToken);
    }
}