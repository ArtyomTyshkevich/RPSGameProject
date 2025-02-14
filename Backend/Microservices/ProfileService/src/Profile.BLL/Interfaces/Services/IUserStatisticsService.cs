using Profile.BLL.DTOs;
using Profile.BLL.Enums;

namespace Profile.BLL.Interfaces.Services
{
    public interface IUserStatisticsService
    {
        Task<int> GetLossesCountAsync(Guid playerId, CancellationToken cancellationToken);
        Task<Dictionary<PlayerMoves, int>> GetMoveStatisticsAsync(Guid playerId, CancellationToken cancellationToken);
        Task<double> GetWinRateAsync(Guid playerId, CancellationToken cancellationToken);
        Task<int> GetWinsCountAsync(Guid playerId, CancellationToken cancellationToken);
        Task<UserWithStatistics> CreateUserWithStatistics(Guid playerId, CancellationToken cancellationToken);
    }
}