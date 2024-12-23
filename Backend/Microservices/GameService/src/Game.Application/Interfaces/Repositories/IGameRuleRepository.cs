using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Application.Interfaces.Repositories
{
    public interface IGameRuleRepository
    {
        Task<List<GameRule>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GameRule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<GameResults> GetResultAsync(PlayerMoves firstPlayerMove, PlayerMoves secondPlayerMove, CancellationToken cancellationToken = default);
    }
}