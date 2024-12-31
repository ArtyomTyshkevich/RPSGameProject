using Profile.DAL.Entities.Mongo;

namespace Profile.BLL.Interfaces.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync(CancellationToken cancellationToken);
        Task<Game> GetGameByIdAsync(string id, CancellationToken cancellationToken);
        Task AddGameAsync(Game game, CancellationToken cancellationToken);
        Task UpdateGameAsync(Game game, CancellationToken cancellationToken);
        Task DeleteGameAsync(string id, CancellationToken cancellationToken);
    }
}
