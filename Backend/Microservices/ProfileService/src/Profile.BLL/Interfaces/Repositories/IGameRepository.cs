using Profile.BLL.Enums;
using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Interfaces.Repositories
{
    public interface IGameRepository
    {
        Task AddGameAsync(Game game, CancellationToken cancellationToken = default);
        Task DeleteGameAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken = default);
        Task<Game> GetGameByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateGameAsync(Game game, CancellationToken cancellationToken = default);
        Task<IEnumerable<Game>> GetAllUserGamesAsync(string userId, CancellationToken cancellationToken = default);
    }
}