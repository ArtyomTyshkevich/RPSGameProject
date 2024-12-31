using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken)
        {
            return await _gameRepository.GetAllGamesAsync(cancellationToken);
        }

        public async Task<Game> GetGameByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _gameRepository.GetGameByIdAsync(id, cancellationToken);
        }

        public async Task AddGameAsync(Game game, CancellationToken cancellationToken)
        {
            await _gameRepository.AddGameAsync(game, cancellationToken);
        }

        public async Task UpdateGameAsync(Game game, CancellationToken cancellationToken)
        {
            await _gameRepository.UpdateGameAsync(game, cancellationToken);
        }

        public async Task DeleteGameAsync(string id, CancellationToken cancellationToken)
        {
            await _gameRepository.DeleteGameAsync(id, cancellationToken);
        }
    }
}