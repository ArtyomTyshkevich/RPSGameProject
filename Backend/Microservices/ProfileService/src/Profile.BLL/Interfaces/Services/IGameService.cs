using Profile.BLL.DTOs;
using RPSGame.Broker.Events;

namespace Profile.BLL.Interfaces.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDTO>> GetAllGamesAsync(CancellationToken cancellationToken);
        Task<GameDTO> GetGameByIdAsync(string id, CancellationToken cancellationToken);
        Task DeleteGameAsync(string id, CancellationToken cancellationToken);
        Task AddGameAsync(GameResultDto gameResultDto, CancellationToken cancellationToken);
}
       
