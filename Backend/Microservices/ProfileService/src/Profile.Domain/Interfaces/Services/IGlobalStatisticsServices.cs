using Profile.BLL.Enums;

namespace Profile.BLL.Interfaces.Services
{
    public interface IGlobalStatisticsServices
    {
        Task<string?> GetMostUsedMoveAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<PlayerMoves, int>> GetMoveUsageStatisticsAsync(CancellationToken cancellationToken = default);
        Task<Dictionary<PlayerMoves, double>> GetMoveWinRateAsync(CancellationToken cancellationToken = default);
    }
}