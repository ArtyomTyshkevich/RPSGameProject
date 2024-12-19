using Chat.Data.Context;
using Game.Application.Interfaces.Repositories;
using Game.Domain.Entities;
using Game.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Game.Data.Repositories
{
    public class GameRoolRepository :  IGameRoolRepository
    {
        private readonly GameDbContext _gameDbContext;

        public GameRoolRepository(GameDbContext context)
        {
            _gameDbContext = context;
        }
        public async Task<GameRool> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.GameRools
                .FirstAsync(rool => rool.Id == id, cancellationToken);
        }

        public async Task<List<GameRool>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _gameDbContext.Set<GameRool>()
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
