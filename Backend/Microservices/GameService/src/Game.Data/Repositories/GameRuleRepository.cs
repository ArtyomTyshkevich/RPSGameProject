using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Repositories
{
    public class GameRuleRepository :  IGameRuleRepository
    {
        private readonly GameDbContext _gameDbContext;

        public GameRuleRepository(GameDbContext context)
        {
            _gameDbContext = context;
        }
        public async Task<GameRule> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.GameRools
                .FirstAsync(rool => rool.Id == id, cancellationToken);
        }

        public async Task<List<GameRule>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Set<GameRule>()
                .ToListAsync(cancellationToken);
        }
        public async Task<GameResults> GetResultAsync(PlayerMoves firstPlayerMove, PlayerMoves secondPlayerMove, CancellationToken cancellationToken = default)
        {
            var rool = await _gameDbContext.GameRools
              .FirstAsync(rool => rool.FirstPlayerMove == firstPlayerMove && rool.SecondPlayerMove == secondPlayerMove, cancellationToken);

            return rool.GameResults;
        }
    }
}
