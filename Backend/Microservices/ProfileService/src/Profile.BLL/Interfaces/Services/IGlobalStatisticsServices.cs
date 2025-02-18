using Profile.BLL.DTOs;
using Profile.BLL.Enums;

namespace Profile.BLL.Interfaces.Services
{
    public interface IGlobalStatisticsServices
    {
        Task<string?> GetMostUsedMoveAsync(IEnumerable<GameDTO> games, CancellationToken cancellationToken = default);
        Task<Dictionary<PlayerMoves, int>> GetMoveUsageStatisticsAsync(IEnumerable<GameDTO> games, CancellationToken cancellationToken = default);
        Task<Dictionary<PlayerMoves, double>> GetMoveWinRateAsync(IEnumerable<GameDTO> games, CancellationToken cancellationToken = default);
    }
}