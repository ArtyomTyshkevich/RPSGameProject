using Profile.BLL.DTOs;
using Profile.BLL.Enums;

namespace Profile.BLL.Interfaces.Services
{
    public interface IUserStatisticsService
    {
        Task<int> GetLossesCountAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken);
        Task<Dictionary<PlayerMoves, int>> GetMoveStatisticsAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken);
        Task<double> GetWinRateAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken);
        Task<int> GetWinsCountAsync(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken);
        Task<UserWithStatistics> CreateUserWithStatistics(IEnumerable<GameDTO> games, Guid playerId, CancellationToken cancellationToken);
        Task<IEnumerable<GameDTO>> GetAllUserGames(Guid playerId, CancellationToken cancellationToken);
    }
}