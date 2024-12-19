using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Repositories
{
    public interface IGameRoolRepository
    {
        Task<List<GameRool>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GameRool> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GameResults> GetResultAsync(PlayerMoves firstPlayerMove, PlayerMoves secondPlayerMove, CancellationToken cancellationToken = default);
    }
}